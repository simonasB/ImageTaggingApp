using System;
using System.Collections.Generic;
using System.Linq;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Services;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace ImageTaggingApp.Tests {
    [TestFixture]
    [Category(TestCategory.UNIT)]
    public class ImageTagsSavingToDatabaseServiceTests_Unit {
        private IDocumentStore _store;

        [SetUp]
        public void EachTestSetup() {
            _store = new EmbeddableDocumentStore {
                RunInMemory = true
            }.Initialize();
        }

        [Test]
        public void ValidateTaggedImagesAreSaved() {
            ImageTagsSavingToDatabaseService service = new ImageTagsSavingToDatabaseService(_store);
            var taggedImagesExpected = new List<Image>();
            var random = new Random();
            for (int i = 0; i < 20; i++) {
                var image = new Image {
                    Path = "ImagesPath" + i,
                    ImageMetadata = new ImageMetadata {
                        Tags = new List<Tag> {
                            new Tag {Name = "Name1_" + i, Probability = random.NextDouble()},
                            new Tag {Name = "Name2_" + i, Probability = random.NextDouble()}
                        }
                    }
                };

                taggedImagesExpected.Add(image);
            }

            service.Save(taggedImagesExpected);

            IList<Image> taggedImagesActual;

            using (IDocumentSession session = _store.OpenSession()) {
                taggedImagesActual = session.Query<Image>().ToList();
            }

            Helpers.AssertAreTwoTaggedImagesListsEqual(taggedImagesExpected, taggedImagesActual);
        }

        [TearDown]
        public void EachTestTeardown() {
            _store.Dispose();
        }
    }
}
