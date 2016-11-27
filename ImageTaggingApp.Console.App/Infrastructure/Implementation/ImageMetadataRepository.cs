using ImageTaggingApp.Console.App.Common.Utils;
using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.Infrastructure.Implementation {
    public class ImageMetadataRepository : IImageMetadataRepository{
        public void Add(IImageMetadata imageMetadata) {
            throw new System.NotImplementedException();
        }

        public Option<IImageMetadata> Find(string imagePath) {
            throw new System.NotImplementedException();
        }
    }
}
