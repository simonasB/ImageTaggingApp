using ImageTaggingApp.Console.App.Application.Implementation;
using ImageTaggingApp.Console.App.Application.Interfaces;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Domain.Implementation;
using ImageTaggingApp.Console.App.Domain.Interfaces;
using ImageTaggingApp.Console.App.Infrastructure.Implementation;
using ImageTaggingApp.Console.App.Presentation.Interfaces;
using Microsoft.Practices.Unity;

namespace ImageTaggingApp.Console.App.Common.IOC {
    public class ContainerBootstrapper {
        // Not sure how to register types correctly. When creating any API object we need to pass API key.
        // That api key will come from console commands.
        // Just adding this for proof of concept
        public static void RegisterTypes(IUnityContainer container) {
            container
                .RegisterType<IApplicationServices, ApplicationServices>()
                .RegisterType<IDomainServices, DomainServices>()
                .RegisterType<IImageTaggingApi, ClarifaiApi>("clarifai")
                .RegisterType<IImageTaggingApi, GoogleVisionApi>("google")
                .RegisterType<IImageTaggingApi, MicrosoftVisionApi>("microsoft")
                .RegisterType<IImageRepository, ImageRepository>()
                .RegisterType<IImageMetadataRepository, ImageMetadataRepository>();
        }
    }
}
