using ImageTaggingApp.Console.App.Common.Utils;
using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.Infrastructure.Implementation {
    public class ImageRepository : IImageRepository{
        public void Add(IImage image) {
            throw new System.NotImplementedException();
        }

        public Option<IImage> Find(string imagePath) {
            throw new System.NotImplementedException();
        }
    }
}
