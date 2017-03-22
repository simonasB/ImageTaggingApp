using System;
using System.Runtime.InteropServices;
using System.Threading;
using CommandLine;
using ImageTaggingApp.Console.App.Commands;
using ImageTaggingApp.Console.App.Common.IOC;
using ImageTaggingApp.Console.App.Services;
using Microsoft.Practices.Unity;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Common.Utils;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App {
    public class Program {
        public static void Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
            var result = Parser.Default.ParseArguments<TagCommandOptions>(args)
                     .MapResult(InitTagCommand, errs => 1);
            System.Console.ReadLine();
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e) {
            System.Console.WriteLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }

        private static readonly CancellationToken _token = new CancellationToken();

        private static int InitTagCommand(TagCommandOptions options) {
            if (options.MinTagConfidence <= 0) {
                throw new Exception("Minimum Tag confidence should be greater than 0.");
            }
            var configFile =
                UnityConfig.GetConfiguredContainer()
                    .Resolve<IConfigFileDeseralizationService<IConfigFile>>(options.API.ToString())
                    .Deserialize(options.ConfigFilePath);

            var imageTaggingApi =
                UnityConfig.GetConfiguredContainer()
                    .Resolve<IImageTaggingApi>(options.API.ToString(),
                        new ParameterOverride("subscriptionKey", configFile.SubscriptionKey));

            var imageTaggingApiExternalResourcesService =
                UnityConfig.GetConfiguredContainer()
                    .Resolve<IImageTagsSavingToExternalResourcesService>(options.Destination.ToString());

            var tagCommand = UnityConfig.GetConfiguredContainer()
                .Resolve<ICommand>(CommandNames.Tag, new ResolverOverride[] {
                    new ParameterOverride("imagesGlobPath", options.ImagesPath),
                    new ParameterOverride("minConfidenceToDetectBlur", options.MinBlurConfidence),
                    new DependencyOverrides {
                        {typeof(IImageTaggingApi), imageTaggingApi}, {
                            typeof(IImageTagsSavingToExternalResourcesService),
                            imageTaggingApiExternalResourcesService
                        },
                        {typeof(CancellationToken), _token}
                    }
                });
            tagCommand.Execute();
            return 0;
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(_token)) {
                        cts.Cancel();
                    }
                    return false;
                default:
                    return false;
            }
        }
    }
}
