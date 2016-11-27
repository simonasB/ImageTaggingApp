using ImageTaggingApp.Console.App.Domain.Interfaces;
using ImageTaggingApp.Console.App.Presentation.Interfaces;

namespace ImageTaggingApp.Console.App.Application.Interfaces {
    public interface IDomainServices {
        void Tag(IImage image);
    }
}
