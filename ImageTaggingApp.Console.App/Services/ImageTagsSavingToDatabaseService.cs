using System.Collections.Generic;
using ImageTaggingApp.Console.App.Entities;
using Raven.Client;

namespace ImageTaggingApp.Console.App.Services {
    public class ImageTagsSavingToDatabaseService : IImageTagsSavingToExternalResourcesService {
        private readonly IDocumentStore _store;

        public ImageTagsSavingToDatabaseService(IDocumentStore store) {
            _store = store;
        }

        public void Save(IEnumerable<Image> taggedImages) {
            using (IDocumentSession session = _store.OpenSession()) {
                foreach (Image taggedImage in taggedImages) {
                    session.Store(taggedImage);
                }
                session.SaveChanges();
            }
        }
    }
}
