using System;
using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.APIs {
    public class GoogleVisionApi : IImageTaggingApi {
        private readonly string _apiKey;

        public GoogleVisionApi(string apiKey) {
            _apiKey = apiKey;
        }

        public Task<IImageMetadata> Tag(IImage image) {
            throw new NotImplementedException();
        }
    }
}
