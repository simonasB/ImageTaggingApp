using System;
using System.Collections.Concurrent;
using System.Threading;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Entities;
using log4net;

namespace ImageTaggingApp.Console.App.Services {
    public interface IImageTaggingService {
        void Tag(BlockingCollection<string> imagesPaths, BlockingCollection<Image> imagesToSaveToExternalResources, CancellationTokenSource cts, IProgress<double> progressBar);
    }

    public class ImageTaggingService : IImageTaggingService {
        private readonly IImageTaggingApi _imageTaggingApi;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ImageFilteringService));
        public ImageTaggingService(IImageTaggingApi imageTaggingApi) {
            _imageTaggingApi = imageTaggingApi;
        }

        public void Tag(BlockingCollection<string> imagesPaths, BlockingCollection<Image> imagesToSaveToExternalResources, CancellationTokenSource cts, IProgress<double> progressBar) {
            _logger.Info("Images tagging stage has started.");
            try {
                var token = cts.Token;
                foreach (var imagePath in imagesPaths.GetConsumingEnumerable()) {
                    _logger.Debug($"Starting tagging process for image {imagePath}");
                    if (token.IsCancellationRequested) {
                        _logger.Debug("Cancellation requested for token. Cancelling tagging stage.");
                        break;
                    }
                    var image = new Image {
                        Path = imagePath,
                        ImageMetadata = _imageTaggingApi.Tag(imagePath).Result
                    };
                    imagesToSaveToExternalResources.Add(image, token);
                    _logger.Debug($"Finished tagging stage for image {imagePath}");
                }
                _logger.Info("Images tagging stage has finished successfully.");
            }
            catch (Exception ex) {
                _logger.Error("Images tagging stage did not complete successfully.", ex);
                cts.Cancel();
                throw;
            } finally {
                imagesToSaveToExternalResources.CompleteAdding();
            }
        }
    }
}
