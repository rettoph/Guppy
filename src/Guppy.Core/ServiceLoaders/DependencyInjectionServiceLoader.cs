using Guppy.EntityComponent.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.Utilities;
using Guppy.EntityComponent.Attributes;
using Guppy.EntityComponent.Interfaces;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using Guppy.Attributes;
using Guppy.ServiceLoaders;

namespace Guppy.EntityComponent.ServiceLoaders
{
    [AutoLoad]
    internal sealed class DependencyInjectionServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterService<Settings>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<Settings>();
                });
        }
    }
}
