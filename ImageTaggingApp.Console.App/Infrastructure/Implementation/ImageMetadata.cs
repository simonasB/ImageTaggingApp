using System.Collections.Generic;
using ImageTaggingApp.Console.App.Domain.Implementation;
using ImageTaggingApp.Console.App.Domain.Interfaces;

namespace ImageTaggingApp.Console.App.Infrastructure.Implementation {
    public class ImageMetadata : IImageMetadata{
        public IEnumerable<Tag> Tags { get; }

        public ImageMetadata(IEnumerable<Tag> tags) {
            Tags = tags;
        }
    }
}
