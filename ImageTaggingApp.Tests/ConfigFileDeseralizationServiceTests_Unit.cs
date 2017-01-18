using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ImageTaggingApp.Tests {
    [TestFixture]
    [Category(TestCategory.UNIT)]
    public class ConfigFileDeseralizationServiceTests_Unit {
        [Test]
        public void SpecifiedFilePathIsNullOrWhiteSpace() {
            var mockFileSystem = new MockFileSystem();
            IConfigFileDeseralizationService<IConfigFile> configFileDeseralizationService = new ConfigFileDeseralizationService<IConfigFile>(mockFileSystem);
            Assert.Throws<ArgumentException>(() => configFileDeseralizationService.Deserialize("   "));
        }

        [Test]
        public void SpecifiedFileNotFound() {
            var mockFileSystem = new MockFileSystem();
            IConfigFileDeseralizationService<IConfigFile> configFileDeseralizationService = new ConfigFileDeseralizationService<IConfigFile>(mockFileSystem);
            Assert.Throws<FileNotFoundException>(() => configFileDeseralizationService.Deserialize(@"C:\notexisting"));
        }

        [Test]
        public void CannotDeserializeIncorrectJson() {
            var mockFileSystem = new MockFileSystem();
            var filePathIncorrectSyntax = @"C:\file.jpg";
            mockFileSystem.AddFile(filePathIncorrectSyntax, new MockFileData("{\"SubscriptionKey\":  \"key\""));
            IConfigFileDeseralizationService<IConfigFile> configFileDeseralizationService = new ConfigFileDeseralizationService<MicrosoftConfigFile>(mockFileSystem);
            Assert.Throws<JsonException>(() => configFileDeseralizationService.Deserialize(filePathIncorrectSyntax));
        }
    }
}
