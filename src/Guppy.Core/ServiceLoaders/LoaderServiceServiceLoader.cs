using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LoaderServiceServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            assemblyHelper.Types.GetTypesWithAutoLoadAttribute(typeof(LoaderService<,,>)).ForEach(loaderServiceType =>
            {
                services.RegisterService(loaderServiceType)
                    .SetLifetime(ServiceLifetime.Singleton)
                    .RegisterTypeFactory(loaderServiceType, factory =>
                    {
                        factory.SetMethod(P => Activator.CreateInstance(loaderServiceType));
                    });
            });
        }
    }
}
