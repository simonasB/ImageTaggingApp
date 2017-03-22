using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using log4net;

namespace ImageTaggingApp.Console.App.Services {
    public interface IImageFilteringService {
        bool IsBlurred(string imageFilePath);
    }

    public class ImageFilteringService : IImageFilteringService {
        private readonly double _minConfidenceToDetectBlur;
        // Maybe better to store these constants in configuration file
        // At the moment leaving as this because blur detection algorithm might change.
        private const int ApartureSize = 1;
        private const int DefaultValueForMinLaplace = -32767;
        private const int MaxLaplace = 600;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ImageFilteringService));

        static ImageFilteringService() {
            _logger.Debug(
                "ImageFilteringService paramaters:" +
                $" {nameof(ApartureSize)}: {ApartureSize}," +
                $" {nameof(DefaultValueForMinLaplace)}: {DefaultValueForMinLaplace}," +
                $" {nameof(MaxLaplace)}: {MaxLaplace}");
        }

        public ImageFilteringService(double minConfidenceToDetectBlur) {
            _minConfidenceToDetectBlur = minConfidenceToDetectBlur;
        }

        public bool IsBlurred(string imageFilePath) {
            var maxLaplaceInImage = DefaultValueForMinLaplace;
            _logger.Debug($"Starting blur evaluation for image {imageFilePath}. Mininimum confidence to detect blur: {_minConfidenceToDetectBlur}%");

            try {
                using (var image = new Bitmap(imageFilePath))
                using (var grayImage = new Image<Gray, byte>(image))
                using (var tmpImage = grayImage.Laplace(ApartureSize)) {
                    foreach (var byteValue in tmpImage.Data) {
                        if (byteValue > maxLaplaceInImage) {
                            maxLaplaceInImage = (int) byteValue;
                        }
                    }
                }
            } catch (Exception ex) {
                _logger.Error($"Failed to evaluate blur for image {imageFilePath}", ex);
            }

            var isImageBlurredConfidence = maxLaplaceInImage / (double) MaxLaplace;

            _logger.Debug($"Image is blurred confidence: {isImageBlurredConfidence}%");

            return isImageBlurredConfidence > _minConfidenceToDetectBlur / 100;
        }
    }
}
