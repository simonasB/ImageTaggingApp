using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageTaggingApp.Console.App.Services {
    public interface IGlobService {
        IEnumerable<string> Expand(string path);
    }

    public class GlobService : IGlobService {
        public IEnumerable<string> Expand(string path) {
            if (string.IsNullOrWhiteSpace(path)) {
                throw new ArgumentException("Glob path is not null or whitespace.", nameof(path));
            }
            return Glob.Glob.Expand(path).Select(o => o.FullName).ToList();
        }
    }
}
