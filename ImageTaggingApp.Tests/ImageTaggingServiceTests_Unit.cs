using System;
using System.Collections.Generic;
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
        public void ImagePathsCountIsZero() {
            var imageTaggingApi = Substitute.For<IImageTaggingApi>();
            var imageTaggingService = new ImageTaggingService(imageTaggingApi);
            Assert.Throws<ArgumentException>(() => imageTaggingService.Tag(new string[] {}));
        }

        [Test]
        public void ImagesMetadataIsSetCorrectly() {
            var imagesPaths = new[] {"a.jpg", "b.jpg", "c.png"};
            var taggedImagesExpected = new List<Image>();
            var imageTaggingApi = Substitute.For<IImageTaggingApi>();
            var random = new Random();
            for (int i = 0; i < imagesPaths.Length; i++) {
                var imageMetadata = new ImageMetadata {
                    Tags = new List<Tag> {
                        new Tag {Name = "Name1_" + i, Probability = random.NextDouble()},
                        new Tag {Name = "Name2_" + i, Probability = random.NextDouble()}
                    }
                };
                taggedImagesExpected.Add(new Image {Path = imagesPaths[i], ImageMetadata = imageMetadata});
                imageTaggingApi.Tag(imagesPaths[i]).Returns(imageMetadata);
            }

            var imageTaggingService = new ImageTaggingService(imageTaggingApi);
            var taggedImagesActual = imageTaggingService.Tag(imagesPaths);
            Helpers.AssertAreTwoTaggedImagesListsEqual(taggedImagesExpected, taggedImagesActual);
        }
    }
}
