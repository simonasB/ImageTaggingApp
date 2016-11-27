using System.Collections.Generic;
using ImageTaggingApp.Console.App.Common.Utils;

namespace ImageTaggingApp.Console.App.Domain.Interfaces {
    public interface IImageMetadataRepository {
        void Add(IImageMetadata imageMetadata);
        Option<IImageMetadata> Find(string imagePath);
    }
}
