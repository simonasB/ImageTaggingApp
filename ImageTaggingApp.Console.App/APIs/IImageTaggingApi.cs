using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.APIs {
    public interface IImageTaggingApi {
        Task<IImageMetadata> Tag(IImage image);
    }
}
