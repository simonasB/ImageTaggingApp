using System.Collections.Generic;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Services;

namespace ImageTaggingApp.Console.App.Factories {
    public interface IImageProcessingFactory {
        IReadOnlyCollection<string> SearchImageFilesToBeTagged(string globPath);
        IList<Image> TagImages(IReadOnlyCollection<string> imageFilePaths);
        void SaveImagesMetadataToDestination(IEnumerable<Image> taggedImages);
    }

    public class ImageProcessingFactory : IImageProcessingFactory {
        private readonly IImageFilesSearchingService _imageFilesSearchingService;
        private readonly IImageTaggingService _imageTaggingService;
        private readonly IImageTagsSavingToExternalResourcesService _imageTagsSavingToExternalResourcesService;

        public ImageProcessingFactory(IImageFilesSearchingService imageFilesSearchingService, IImageTaggingService imageTaggingService, IImageTagsSavingToExternalResourcesService imageTagsSavingToExternalResourcesService) {
            _imageFilesSearchingService = imageFilesSearchingService;
            _imageTaggingService = imageTaggingService;
            _imageTagsSavingToExternalResourcesService = imageTagsSavingToExternalResourcesService;
        }

        public IReadOnlyCollection<string> SearchImageFilesToBeTagged(string globPath) {
            return _imageFilesSearchingService.SearchForImageFilesToBeTagged(globPath);
        }

        public IList<Image> TagImages(IReadOnlyCollection<string> imageFilePaths) {
            return _imageTaggingService.Tag(imageFilePaths);
        }

        public void SaveImagesMetadataToDestination(IEnumerable<Image> taggedImages) {
            _imageTagsSavingToExternalResourcesService.Save(taggedImages);
        }
    }
}
