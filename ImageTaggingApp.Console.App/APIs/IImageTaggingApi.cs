using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App.APIs {
    public interface IImageTaggingApi {
        Task<ImageMetadata> Tag(string imagePath);
    }
}
