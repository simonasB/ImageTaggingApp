using ImageTaggingApp.Console.App.Application.Implementation;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Domain.Implementation;
using ImageTaggingApp.Console.App.Infrastructure.Implementation;
using Microsoft.Practices.Unity;

namespace ImageTaggingApp.Console.App.Common.IOC {
    public class ContainerBootstrapper {
        // Not sure how to register types correctly. When creating any API object we need to pass API key.
        // That api key will come from console commands.
        // Just adding this for proof of concept
        public static void RegisterTypes(IUnityContainer container) {
            container
                .RegisterType<ApplicationServices, ApplicationServices>()
                .RegisterType<DomainServices, DomainServices>()
                .RegisterType<IImageTaggingApi, ClarifaiApi>("clarifai")
                .RegisterType<IImageTaggingApi, GoogleVisionApi>("google")
                .RegisterType<IImageTaggingApi, MicrosoftVisionApi>("microsoft")
                .RegisterType<ImageRepository, ImageRepository>()
                .RegisterType<ImageMetadataRepository, ImageMetadataRepository>();
        }
    }
}
