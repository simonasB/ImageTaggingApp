using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Services;
using NUnit.Framework;
using Raven.Client;

namespace ImageTaggingApp.Tests {
    [TestFixture]
    [Category(TestCategory.UNIT)]
    public class ImageTagsSavingToDatabaseServiceTests_Unit : MockDatabaseTestBase{
        [Test]
        public void ValidateTaggedImagesAreSaved() {
            ImageTagsSavingToDatabaseService service = new ImageTagsSavingToDatabaseService(Store);
            var taggedImagesExpected = new BlockingCollection<Image>();
            var probabilities = new[] {0.11, 0.22, 0.33, 0.10, 0.02, 0.90, 0.45, 0.75, 0.63, 0.52};
            var probabilities2 = new[] {0.41, 0.92, 0.53, 0.20, 0.62, 0.20, 0.15, 0.95, 0.53, 0.32};
            for (int i = 0; i < 10; i++) {
                var image = new Image {
                    Path = "ImagesPath" + i,
                    ImageMetadata = new ImageMetadata {
                        Tags = new List<Tag> {
                            new Tag {Name = "Name1_" + i, Probability = probabilities[i]},
                            new Tag {Name = "Name2_" + i, Probability = probabilities2[i]}
                        }
                    }
                };

                taggedImagesExpected.Add(image);
            }
            taggedImagesExpected.CompleteAdding();
            service.Save(taggedImagesExpected, new CancellationTokenSource(), new ProgressBar());
            IList<Image> taggedImagesActual;

            using (IDocumentSession session = Store.OpenSession()) {
                taggedImagesActual = session.Query<Image>().ToList();
            }

            Helpers.AssertAreTwoTaggedImagesListsEqual(taggedImagesExpected.ToList(), taggedImagesActual);
        }
    }
}
