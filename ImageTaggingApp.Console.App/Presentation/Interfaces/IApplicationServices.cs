using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.Presentation.Interfaces {
    public interface IApplicationServices {
        void Tag(IImage image);
    }
}
