using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.Infrastructure.Implementation {
    public class Image : IImage {
        public string Path { get; }

        public Image(string path) {
            Path = path;
        }
    }
}
