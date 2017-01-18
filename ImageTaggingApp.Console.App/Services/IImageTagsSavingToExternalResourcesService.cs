using System.Collections.Generic;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App.Services {
    public interface IImageTagsSavingToExternalResourcesService {
        void Save(IEnumerable<Image> taggedImages);
    }
}
