using Guppy.Attributes;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.Lists;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LayerableServiceCollection : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterService<LayerableList>()
                .SetLifetime(ServiceLifetime.Scoped)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<LayerableList>();
                });

            services.RegisterSetup<ILayerable>()
                .SetMethod((l, p, c) =>
                {
                    p.GetService<LayerableList>().TryAdd(l);
                });
        }
    }
}
