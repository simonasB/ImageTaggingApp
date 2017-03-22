using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace ImageTaggingApp.Tests {
    public abstract class MockDatabaseTestBase {
        protected IDocumentStore Store { get; set; }

        [SetUp]
        public virtual void EachTestSetup() {
            Store = new EmbeddableDocumentStore {
                RunInMemory = true
            }.Initialize();
        }

        [TearDown]
        public virtual void EachTestTeardown() {
            Store.Dispose();
        }
    }
}
