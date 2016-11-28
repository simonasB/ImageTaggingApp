using System.IO;
using ImageTaggingApp.Console.App.Application.Implementation;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Common.IOC;
using ImageTaggingApp.Console.App.Domain.Implementation;
using ImageTaggingApp.Console.App.Infrastructure.Implementation;
using Microsoft.Practices.Unity;
using Microsoft.ProjectOxford.Vision;

namespace ImageTaggingApp.Console.App {
    public class Program {
        public static void Main(string[] args) {
            //var applicationServices = UnityConfig.GetConfiguredContainer().Resolve<ApplicationServices>();
            var apiKey = ""; // Enter API key for testing purposes.
            var applicationServices = new ApplicationServices(
                new DomainServices(
                    new MicrosoftVisionApi(apiKey, new VisionServiceClient(apiKey)),
                    new ImageRepository(), new ImageMetadataRepository()));
            var imagePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\nature.jpg";
            applicationServices.Tag(new Image(imagePath));
        }
    }
}
