using System.Collections.Generic;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Factories;

namespace ImageTaggingApp.Console.App.Commands {
    public class TagCommand : ICommand {
        private readonly IImageProcessingFactory _imageProcessingFactory;
        private readonly string _imagesPath;

        public TagCommand(IImageProcessingFactory imageProcessingFactory, string imagesPath) {
            _imageProcessingFactory = imageProcessingFactory;
            _imagesPath = imagesPath;
        }

        public void Execute() {
            IReadOnlyCollection<string> imageFilePaths = _imageProcessingFactory.SearchImageFilesToBeTagged(_imagesPath);
            IList<Image> taggedImages = _imageProcessingFactory.TagImages(imageFilePaths);
            _imageProcessingFactory.SaveImagesMetadataToDestination(taggedImages);
        }
    }
}
