using System;
using Microsoft.Practices.Unity;

namespace ImageTaggingApp.Console.App.Common.IOC {
    public class UnityConfig {
        private static readonly Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            ContainerBootstrapper.RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return _container.Value;
        }
    }
}
