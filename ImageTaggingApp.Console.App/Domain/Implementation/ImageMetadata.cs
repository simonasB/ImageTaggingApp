using System.Collections.Generic;
using ImageTaggingApp.Console.App.Domain.Implementation;

namespace ImageTaggingApp.Console.App.Infrastructure.Implementation {
    public class ImageMetadata {
        public IEnumerable<Tag> Tags { get; }

        public ImageMetadata(IEnumerable<Tag> tags) {
            Tags = tags;
        }
    }
}
