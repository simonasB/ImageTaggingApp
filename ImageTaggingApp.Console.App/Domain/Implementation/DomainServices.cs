using ImageTaggingApp.Console.App.Application.Interfaces;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.Domain.Implementation {
    public class DomainServices : IDomainServices {
        private readonly IImageMetadataRepository _imageMetadataRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IImageTaggingApi _imageTaggingApi;

        public DomainServices(IImageTaggingApi imageTaggingApi, IImageRepository imageRepository, IImageMetadataRepository imageMetadataRepository) {
            _imageTaggingApi = imageTaggingApi;
            _imageRepository = imageRepository;
            _imageMetadataRepository = imageMetadataRepository;
        }

        public void Tag(IImage image) {
            var imageMetadata = _imageTaggingApi.Tag(image).Result;
            //_imageRepository.Add(image);
            //_imageMetadataRepository.Add(imageMetadata.Result);
        }
    }
}
