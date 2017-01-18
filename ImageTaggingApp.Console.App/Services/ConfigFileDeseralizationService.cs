using System;
using System.IO;
using System.IO.Abstractions;
using ImageTaggingApp.Console.App.Entities;
using Newtonsoft.Json;

namespace ImageTaggingApp.Console.App.Services {
    public interface IConfigFileDeseralizationService<out T> where T : IConfigFile {
        T Deserialize(string filePath);
    }

    public class ConfigFileDeseralizationService<T> : IConfigFileDeseralizationService<T> where T : IConfigFile {
        private readonly IFileSystem _fileSystem;

        public ConfigFileDeseralizationService(IFileSystem fileSystem) {
            _fileSystem = fileSystem;
        }

        public ConfigFileDeseralizationService() : this(new FileSystem()) {

        }

        public T Deserialize(string filePath) {
            ValidateFilePath(filePath);
            try {
                return JsonConvert.DeserializeObject<T>(_fileSystem.File.ReadAllText(filePath));
            }
            catch (JsonException ex) {
                throw new JsonException("Could not parse config file to JSON format.", ex);
            }
        }

        private void ValidateFilePath(string filePath) {
            if (string.IsNullOrWhiteSpace(filePath)) {
                throw new ArgumentException("Specified config file path is not specified or whitespace.", nameof(filePath));
            }
            if (!_fileSystem.File.Exists(filePath)) {
                throw new FileNotFoundException("Could not find specified config file.", filePath);
            }
        }
    }
}
