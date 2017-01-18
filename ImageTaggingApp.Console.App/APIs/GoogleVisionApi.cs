using System;
using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App.APIs {
    public class GoogleVisionApi : IImageTaggingApi {
        private readonly string _apiKey;

        public GoogleVisionApi(string apiKey) {
            _apiKey = apiKey;
        }

        public Task<ImageMetadata> Tag(string imagePath) {
            throw new NotImplementedException();
        }
    }
}
