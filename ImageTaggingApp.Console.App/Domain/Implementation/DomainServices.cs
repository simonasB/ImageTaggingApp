using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Infrastructure.Implementation;

namespace ImageTaggingApp.Console.App.Domain.Implementation {
    public class DomainServices {
        private readonly ImageMetadataRepository _imageMetadataRepository;
        private readonly ImageRepository _imageRepository;
        private readonly IImageTaggingApi _imageTaggingApi;

        public DomainServices(IImageTaggingApi imageTaggingApi, ImageRepository imageRepository, ImageMetadataRepository imageMetadataRepository) {
            _imageTaggingApi = imageTaggingApi;
            _imageRepository = imageRepository;
            _imageMetadataRepository = imageMetadataRepository;
        }

        public void Tag(Image image) {
            var imageMetadata = _imageTaggingApi.Tag(image).Result;
            //_imageRepository.Add(image);
            //_imageMetadataRepository.Add(imageMetadata.Result);
        }
    }
}
