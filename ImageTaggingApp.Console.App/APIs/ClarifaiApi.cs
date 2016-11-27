using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.APIs {
    public class ClarifaiApi : IImageTaggingApi {
        private readonly string _apiKey;

        public ClarifaiApi(string apiKey) {
            _apiKey = apiKey;
        }

        public Task<IImageMetadata> Tag(IImage image) {
            throw new System.NotImplementedException();
        }
    }
}
