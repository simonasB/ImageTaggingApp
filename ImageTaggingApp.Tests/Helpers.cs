using System.Collections.Generic;
using System.Linq;
using ImageTaggingApp.Console.App.Entities;
using NUnit.Framework;

namespace ImageTaggingApp.Tests {
    public static class Helpers {
        public static void AssertAreTwoTaggedImagesListsEqual(IList<Image> expected, IList<Image> actual) {
            var sortedExpected = expected.OrderBy(o => o.Path).ToList();
            var sortedActual = actual.OrderBy(o => o.Path).ToList();
            Assert.AreEqual(expected.Count, expected.Count, "Images count should be the same after tagging operation.");
            for (int i = 0; i < expected.Count; i++)
            {
                var taggedImageExpected = sortedExpected[i];
                var taggedImageActual = sortedActual[i];
                Assert.AreEqual(taggedImageExpected.Path, taggedImageActual.Path, "Expected and actual Path for tagged Image does not match.");
                Assert.AreEqual(taggedImageExpected.ImageMetadata.Tags.Count, taggedImageActual.ImageMetadata.Tags.Count, "Expected and actual Tags count for tagged Image does not match.");
                for (int j = 0; j < taggedImageActual.ImageMetadata.Tags.Count; j++)
                {
                    Assert.AreEqual(taggedImageExpected.ImageMetadata.Tags[j].Name, taggedImageActual.ImageMetadata.Tags[j].Name, "Expected and actual Tag name for tagged Image does not match.");
                    Assert.AreEqual(taggedImageExpected.ImageMetadata.Tags[j].Probability, taggedImageActual.ImageMetadata.Tags[j].Probability, 0.00000001, "Expected and actual Tag probability for tagged Image does not match.");
                }
            }
        }
    }
}
