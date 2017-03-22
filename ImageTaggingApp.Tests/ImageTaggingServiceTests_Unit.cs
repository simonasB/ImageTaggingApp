using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Services;
using NSubstitute;
using NUnit.Framework;

namespace ImageTaggingApp.Tests {
    [TestFixture]
    [Category(TestCategory.UNIT)]
    public class ImageTaggingServiceTests_Unit {
        [Test]
        public void ImagesMetadataIsSetCorrectly() {
            var imagesPaths = new[] {"a.jpg", "b.jpg", "c.png"};
            var imagesPathsInBlockingColl = new BlockingCollection<string>();
            var taggedImagesExpected = new List<Image>();
            var imageTaggingApi = Substitute.For<IImageTaggingApi>();
            var probabilities = new[] { 0.11, 0.22, 0.33, 0.10, 0.02, 0.90, 0.45, 0.75, 0.63, 0.52 };
            var probabilities2 = new[] { 0.41, 0.92, 0.53, 0.20, 0.62, 0.20, 0.15, 0.95, 0.53, 0.32 };

            for (int i = 0; i < imagesPaths.Length; i++) {
                var imageMetadata = new ImageMetadata {
                    Tags = new List<Tag> {
                        new Tag {Name = "Name1_" + i, Probability = probabilities[i]},
                        new Tag {Name = "Name2_" + i, Probability = probabilities2[i]}
                    }
                };
                taggedImagesExpected.Add(new Image {Path = imagesPaths[i], ImageMetadata = imageMetadata});
                imageTaggingApi.Tag(imagesPaths[i]).Returns(imageMetadata);
                imagesPathsInBlockingColl.Add(imagesPaths[i]);
            }
            imagesPathsInBlockingColl.CompleteAdding();

            var imageTaggingService = new ImageTaggingService(imageTaggingApi);
            var imagesToSaveToExternalResources = new BlockingCollection<Image>();
            imageTaggingService.Tag(imagesPathsInBlockingColl, imagesToSaveToExternalResources, new CancellationTokenSource(), new ProgressBar());

            Helpers.AssertAreTwoTaggedImagesListsEqual(taggedImagesExpected, imagesToSaveToExternalResources.ToList());
        }
    }
}
