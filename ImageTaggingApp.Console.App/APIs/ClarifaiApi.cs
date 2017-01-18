using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App.APIs {
    public class ClarifaiApi : IImageTaggingApi {
        private readonly string _apiKey;

        public ClarifaiApi(string apiKey) {
            _apiKey = apiKey;
        }

        public Task<ImageMetadata> Tag(string imagePath) {
            throw new System.NotImplementedException();
        }
    }
}
