using ImageTaggingApp.Console.App.Common.Utils;

namespace ImageTaggingApp.Console.App.Domain.Interfaces {
    public interface IImageRepository {
        void Add(IImage image);
        Option<IImage> Find(string imagePath);
    }
}
