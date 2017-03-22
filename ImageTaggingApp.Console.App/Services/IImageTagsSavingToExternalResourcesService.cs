using System;
using System.Collections.Concurrent;
using System.Threading;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App.Services {
    public interface IImageTagsSavingToExternalResourcesService {
        void Save(BlockingCollection<Image> imagesToSaveToExternalResources, CancellationTokenSource cts, IProgress<double> progressBar);
    }
}
