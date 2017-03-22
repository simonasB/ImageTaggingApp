using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using log4net;

namespace ImageTaggingApp.Console.App.Services {
    public interface IImagesPreprocessService {
        void Preprocess(IReadOnlyCollection<string> imagesPaths, BlockingCollection<string> imagesToTag, CancellationTokenSource cts, IProgress<double> progressBar);
    }

    public class ImagesPreprocessService : IImagesPreprocessService {
        private readonly IImageFilteringService _imageFilteringService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ImageFilteringService));

        public ImagesPreprocessService(IImageFilteringService imageFilteringService) {
            _imageFilteringService = imageFilteringService;
        }
        public void Preprocess(IReadOnlyCollection<string> imagesPaths, BlockingCollection<string> imagesToTag, CancellationTokenSource cts, IProgress<double> progressBar) {
            _logger.Info("Images preprocessing is started.");
            try
            {
                var token = cts.Token;
                foreach (var imagePath in imagesPaths) {
                    _logger.Debug($"Starting image preprocessing for image {imagePath}");
                    if (token.IsCancellationRequested) {
                        _logger.Debug("Cancellation requested for token. Cancelling preprocessing stage.");
                        break;
                    }
                    if (!_imageFilteringService.IsBlurred(imagePath)) {
                        imagesToTag.Add(imagePath, token);
                    }
                    _logger.Debug($"Finished preprocess stage for image {imagePath}");
                }
                _logger.Info("Images preprocessing has finished successfully.");
            }
            catch (Exception ex) {
                _logger.Error("Images preprocess stage did not complete successfully.", ex);
                cts.Cancel();
                throw;
            } finally {
                imagesToTag.CompleteAdding();
            }
        }
    }
}
