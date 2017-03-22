using System;
using System.Collections.Generic;
using System.IO;
using log4net;

namespace ImageTaggingApp.Console.App.Services {
    public interface IImageFilesSearchingService {
        IReadOnlyCollection<string> SearchForImageFilesToBeTagged(string globPath, IProgress<double> progressBar);
    }

    public class ImageFilesSearchingService : IImageFilesSearchingService {
        private readonly IGlobService _globService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ImageFilteringService));

        public ImageFilesSearchingService(IGlobService globService) {
            _globService = globService;
        }

        public IReadOnlyCollection<string> SearchForImageFilesToBeTagged(string globPath, IProgress<double> progressBar) {
            _logger.Debug($"Starting to search image files to tag with path {globPath}");
            if (string.IsNullOrWhiteSpace(globPath)) {
                throw new ArgumentException("Glob path is null or is whitespace.", nameof(globPath));
            }
            IReadOnlyCollection<string> filteredFilePaths = (IReadOnlyCollection<string>) _globService.Expand(globPath);
            if (filteredFilePaths.Count == 0) {
                throw new FileNotFoundException($"No files found using specified path: {globPath}");
            }
            _logger.Debug($"Found {filteredFilePaths.Count} images to tag.");
            return filteredFilePaths;
        }
    }
}
