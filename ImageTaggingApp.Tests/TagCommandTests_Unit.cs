using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Commands;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Services;
using NSubstitute;
using NUnit.Framework;
using Raven.Client;

namespace ImageTaggingApp.Tests {
    [TestFixture]
    [Category(TestCategory.UNIT)]
    public class TagCommandTests_Unit : MockDatabaseTestBase {
        [Test]
        public void ExecutePipelineStages() {
            var imagePaths = new[] { "a.jpg", "b.jpg", "c.jpg" };
            var imageFilesSearchingService = Substitute.For<IImageFilesSearchingService>();
            imageFilesSearchingService
                .SearchForImageFilesToBeTagged(Arg.Any<string>(), Arg.Any<ProgressBar>())
                .Returns(imagePaths);

            var imageFilteringService = Substitute.For<IImageFilteringService>();
            imageFilteringService.IsBlurred(Arg.Any<string>()).Returns(false);
            var imagesPreprocessService = new ImagesPreprocessService(imageFilteringService);

            var imageTaggingApi = Substitute.For<IImageTaggingApi>();
            var tags = new List<Tag> {new Tag {Name = "Tag", Probability = 0.11}};
            imageTaggingApi.Tag(Arg.Any<string>())
                .Returns(new ImageMetadata {Tags = tags });
            var imageTaggingService = new ImageTaggingService(imageTaggingApi);

            var imageTagsSavingToExternalResourcesService = new ImageTagsSavingToDatabaseService(Store);

            var cancellationToken = new CancellationToken();
           
            IList<Image> taggedImagesExpected = new List<Image>();
            foreach (var imagePath in imagePaths) {
                taggedImagesExpected.Add(new Image {Path = imagePath, ImageMetadata = new ImageMetadata {Tags = tags}});
            }

            var tagCommand = new TagCommand(imageFilesSearchingService, imagesPreprocessService, imageTaggingService, imageTagsSavingToExternalResourcesService, new ProgressBar(), "path.*", cancellationToken);
            tagCommand.Execute();

            IList<Image> taggedImagesActual;

            using (IDocumentSession session = Store.OpenSession()) {
                taggedImagesActual = session.Query<Image>().ToList();
            }

            Helpers.AssertAreTwoTaggedImagesListsEqual(taggedImagesExpected, taggedImagesActual);
        }
    }
}
