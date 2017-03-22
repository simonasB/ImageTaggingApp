using System;
using System.Collections.Generic;
using System.IO;
using ImageTaggingApp.Console.App.Services;
using NSubstitute;
using NUnit.Framework;

namespace ImageTaggingApp.Tests {
    [TestFixture]
    [Category(TestCategory.UNIT)]
    public class ImageFilesSearchingServiceTests_Unit {
        [Test]
        public void GlobPathSpecifiedIsNullOrWhitespace() {
            var globService = Substitute.For<IGlobService>();
            var imageFilesSearchingService = new ImageFilesSearchingService(globService);
            Assert.Throws<ArgumentException>(() => imageFilesSearchingService.SearchForImageFilesToBeTagged(null, new ProgressBar()));
        }

        [Test]
        public void NoFilesFound() {
            var globService = Substitute.For<IGlobService>();
            globService.Expand(Arg.Any<string>()).Returns(new string[] {});
            var imageFilesSearchingService = new ImageFilesSearchingService(globService);
            Assert.Throws<FileNotFoundException>(() => imageFilesSearchingService.SearchForImageFilesToBeTagged(@"C:\a*\*", new ProgressBar()));
        }

        [Test]
        public void FilesFound() {
            var globService = Substitute.For<IGlobService>();
            IEnumerable<string> imageFilesToTagExpected = new[] {"a.jpg", "b.jpg", "c.jpg"};
            globService.Expand(Arg.Any<string>()).Returns(imageFilesToTagExpected);
            var imageFilesSearchingService = new ImageFilesSearchingService(globService);
            var imageFilesToTagActual = imageFilesSearchingService.SearchForImageFilesToBeTagged(@"C:\a*\*", new ProgressBar());

            Assert.That(imageFilesToTagActual, Is.EquivalentTo(imageFilesToTagExpected), "Returned list of file paths from service is incorrect.");
        }
    }
}
