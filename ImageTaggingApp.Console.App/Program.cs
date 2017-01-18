using System;
using CommandLine;
using ImageTaggingApp.Console.App.Commands;
using ImageTaggingApp.Console.App.Common.IOC;
using ImageTaggingApp.Console.App.Services;
using Microsoft.Practices.Unity;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App {
    public class Program {
        public static void Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            var result = Parser.Default.ParseArguments<TagCommandOptions>(args)
                     .MapResult(InitTagCommand, errs => 1);
            System.Console.ReadLine();
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e) {
            System.Console.WriteLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }

        private static int InitTagCommand(TagCommandOptions options) {
            if (options.MinConfidence <= 0) {
                throw new Exception("Minimum confidence should be greater than 0.");
            }
            var configFile = UnityConfig.GetConfiguredContainer().Resolve<IConfigFileDeseralizationService<IConfigFile>>(options.API.ToString()).Deserialize(options.ConfigFilePath);
            var imageTaggingApi = UnityConfig.GetConfiguredContainer().Resolve<IImageTaggingApi>(options.API.ToString(), new ParameterOverride("subscriptionKey", configFile.SubscriptionKey));
            var imageTaggingApiExternalResourcesService = UnityConfig.GetConfiguredContainer().Resolve<IImageTagsSavingToExternalResourcesService>(options.Destination.ToString());
            var tagCommand = UnityConfig.GetConfiguredContainer().Resolve<ICommand>(CommandsNames.Tag, new ResolverOverride[] {
                new ParameterOverride("imagesPath", options.ImagesPath),
                new DependencyOverrides {
                    {typeof (IImageTaggingApi), imageTaggingApi},
                    {typeof (IImageTagsSavingToExternalResourcesService), imageTaggingApiExternalResourcesService}
                }
            });
            tagCommand.Execute();
            return 0;
        }
    }
}
