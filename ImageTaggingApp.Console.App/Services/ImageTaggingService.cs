using System;
using System.Collections.Generic;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App.Services {
    public interface IImageTaggingService {
        IList<Image> Tag(IReadOnlyCollection<string> imagesPaths);
    }

    public class ImageTaggingService : IImageTaggingService {
        private readonly IImageTaggingApi _imageTaggingApi;

        public ImageTaggingService(IImageTaggingApi imageTaggingApi) {
            _imageTaggingApi = imageTaggingApi;
        }

        public IList<Image> Tag(IReadOnlyCollection<string> imagesPaths) {
            if (imagesPaths.Count == 0) {
                throw new ArgumentException("Image paths count is equal to zero.", nameof(imagesPaths));
            }
            var taggedImages = new List<Image>();
            foreach (var imagePath in imagesPaths) {
                var image = new Image {
                    Path = imagePath,
                    ImageMetadata = _imageTaggingApi.Tag(imagePath).Result
                };
                taggedImages.Add(image);
            }
            return taggedImages;
        }
    }
}
