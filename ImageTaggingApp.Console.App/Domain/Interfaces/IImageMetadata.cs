using System.Collections.Generic;
using ImageTaggingApp.Console.App.Domain.Implementation;

namespace ImageTaggingApp.Console.App.Domain.Interfaces {
    public interface IImageMetadata {
        IEnumerable<Tag> Tags { get; }
    }
}
