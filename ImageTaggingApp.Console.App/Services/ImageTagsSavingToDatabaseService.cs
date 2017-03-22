using System;
using System.Collections.Concurrent;
using System.Threading;
using ImageTaggingApp.Console.App.Entities;
using log4net;
using Raven.Client;

namespace ImageTaggingApp.Console.App.Services {
    public class ImageTagsSavingToDatabaseService : IImageTagsSavingToExternalResourcesService {
        private readonly IDocumentStore _store;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ImageFilteringService));

        public ImageTagsSavingToDatabaseService(IDocumentStore store) {
            _store = store;
        }

        public void Save(BlockingCollection<Image> imagesToSaveToExternalResources, CancellationTokenSource cts, IProgress<double> progressBar) {
            _logger.Info("Images saving to DB stage has started.");
            IDocumentSession session = null;
            try {
                session = _store.OpenSession();
                var token = cts.Token;
                foreach (var taggedImage in imagesToSaveToExternalResources.GetConsumingEnumerable()) {
                    _logger.Debug($"Starting image {taggedImage.Path} saving to DB stage.");
                    if (token.IsCancellationRequested) {
                        break;
                    }
                    session.Store(taggedImage);
                    _logger.Debug($"Image {taggedImage.Path} has been stored to session.");
                }
                _logger.Info("Images saving to DB stage has finished successfully.");
            }
            catch (Exception ex) {
                _logger.Error("Images saving to DB stage did not complete successfully.", ex);
                cts.Cancel();
                throw;
            }
            finally {
                session?.SaveChanges();
                session?.Dispose();
                imagesToSaveToExternalResources.CompleteAdding();
            }
        }
    }
}
