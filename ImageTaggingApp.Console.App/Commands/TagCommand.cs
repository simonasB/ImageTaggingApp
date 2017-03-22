using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Services;
using log4net;

namespace ImageTaggingApp.Console.App.Commands {
    public class TagCommand : ICommand {
        private readonly IImageFilesSearchingService _imageFilesSearchingService;
        private readonly IImagesPreprocessService _imagesPreprocessService;
        private readonly IImageTaggingService _imageTaggingService;
        private readonly IImageTagsSavingToExternalResourcesService _imageTagsSavingToExternalResourcesService;
        private readonly IProgress<double> _progressBar;
        private readonly string _imagesGlobPath;
        private readonly CancellationToken _token;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ImageFilteringService));

        public TagCommand(IImageFilesSearchingService imageFilesSearchingService,
            IImagesPreprocessService imagesPreprocessService,
            IImageTaggingService imageTaggingService,
            IImageTagsSavingToExternalResourcesService imageTagsSavingToExternalResourcesService,
            IProgress<double> progressBar,
            string imagesGlobPath,
            CancellationToken token) {
            _imageFilesSearchingService = imageFilesSearchingService;
            _imagesPreprocessService = imagesPreprocessService;
            _imageTaggingService = imageTaggingService;
            _imageTagsSavingToExternalResourcesService = imageTagsSavingToExternalResourcesService;
            _progressBar = progressBar;
            _imagesGlobPath = imagesGlobPath;
            _token = token;
        }

        public void Execute() {
            _logger.Info("Starting to execute Tag command.");
            IReadOnlyCollection<string> imagePaths =
                _imageFilesSearchingService.SearchForImageFilesToBeTagged(_imagesGlobPath, _progressBar);
            InvokePipeline(imagePaths);
            _logger.Info("Tag command execution has finished successfully.");
        }

        private void InvokePipeline(IReadOnlyCollection<string> imagePaths) {
            using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(_token)) {
                int limit = 50;
                var imagesToTag = new BlockingCollection<string>(limit);
                var imagesToSaveToExternalResources = new BlockingCollection<Image>(limit);

                var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);

                var preprocessImagesTask =
                    taskFactory.StartNew(() => _imagesPreprocessService.Preprocess(imagePaths, imagesToTag, cts, _progressBar));
                var tagImagesTask =
                    taskFactory.StartNew(
                        () =>
                            _imageTaggingService.Tag(imagesToTag, imagesToSaveToExternalResources, cts,
                                _progressBar));
                var saveImageTagsToExternalResources =
                    taskFactory.StartNew(
                        () =>
                            _imageTagsSavingToExternalResourcesService.Save(imagesToSaveToExternalResources, cts,
                                _progressBar));

                Task.WaitAll(preprocessImagesTask, tagImagesTask, saveImageTagsToExternalResources);
            }
        }
    }
}
