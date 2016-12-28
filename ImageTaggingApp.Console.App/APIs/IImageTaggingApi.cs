using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Infrastructure.Implementation;

namespace ImageTaggingApp.Console.App.APIs {
    public interface IImageTaggingApi {
        Task<ImageMetadata> Tag(Image image);
    }
}
