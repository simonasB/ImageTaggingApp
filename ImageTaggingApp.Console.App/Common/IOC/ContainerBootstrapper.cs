using System;
using System.IO.Abstractions;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Commands;
using ImageTaggingApp.Console.App.Database;
using ImageTaggingApp.Console.App.Entities;
using ImageTaggingApp.Console.App.Services;
using Microsoft.Practices.Unity;
using Microsoft.ProjectOxford.Vision;

namespace ImageTaggingApp.Console.App.Common.IOC {
    public class ContainerBootstrapper {
        public static void RegisterTypes(IUnityContainer container) {
            container.RegisterInstance(DocumentStoreHolder.Store);

            container
                .RegisterType<ICommand, TagCommand>(CommandNames.Tag)
                .RegisterType<IImageFilesSearchingService, ImageFilesSearchingService>()
                .RegisterType<IImagesPreprocessService, ImagesPreprocessService>()
                .RegisterType<IImageTaggingService, ImageTaggingService>()
                .RegisterType<IImageTagsSavingToExternalResourcesService, ImageTagsSavingToDatabaseService>(
                    TagDestinationType.Database.ToString())
                .RegisterType<IImageTagsSavingToExternalResourcesService, ImageTagsSavingToImageMetadataService>(
                    TagDestinationType.ImageMetadata.ToString())
                .RegisterType<IImageTaggingApi, ClarifaiApi>(TaggingAPIType.Clarifai.ToString())
                .RegisterType<IImageTaggingApi, GoogleVisionApi>(TaggingAPIType.Google.ToString())
                .RegisterType<IImageTaggingApi, MicrosoftVisionApi>(TaggingAPIType.Microsoft.ToString())
                .RegisterType<IVisionServiceClient, VisionServiceClient>(new InjectionConstructor(typeof(string)))
                .RegisterType<IConfigFileDeseralizationService<IConfigFile>, ConfigFileDeseralizationService<MicrosoftConfigFile>>(TaggingAPIType.Microsoft.ToString())
                .RegisterType<IFileSystem, FileSystem>()
                .RegisterType<IGlobService, GlobService>()
                .RegisterType<IImageFilteringService, ImageFilteringService>()
                .RegisterType<IProgress<double>, ProgressBar>(new ContainerControlledLifetimeManager());
        }
    }
}
