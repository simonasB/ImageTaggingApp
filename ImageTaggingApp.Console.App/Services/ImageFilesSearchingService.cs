using System;
using System.Collections.Generic;
using System.IO;

namespace ImageTaggingApp.Console.App.Services {
    public interface IImageFilesSearchingService {
        IReadOnlyCollection<string> SearchForImageFilesToBeTagged(string globPath);
    }

    public class ImageFilesSearchingService : IImageFilesSearchingService {
        private readonly IGlobService _globService;

        public ImageFilesSearchingService(IGlobService globService) {
            _globService = globService;
        }

        public IReadOnlyCollection<string> SearchForImageFilesToBeTagged(string globPath) {
            if (string.IsNullOrWhiteSpace(globPath)) {
                throw new ArgumentException("Glob path is null or is whitespace.", nameof(globPath));
            }
            IReadOnlyCollection<string> filteredFilePaths = (IReadOnlyCollection<string>) _globService.Expand(globPath);
            if (filteredFilePaths.Count == 0) {
                throw new FileNotFoundException($"No files found using specified path: {globPath}");
            }
            return filteredFilePaths;
        }
    }
}
