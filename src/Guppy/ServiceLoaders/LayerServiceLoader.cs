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
    internal sealed class LayerServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            // Configure services...
            services.RegisterService<Layer>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<Layer>();
                });

            services.RegisterService<LayerList>()
                .SetLifetime(ServiceLifetime.Scoped)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<LayerList>();
                });

            services.RegisterService<OrderableList<ILayerable>>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<OrderableList<ILayerable>>();
                });
        }
    }
}
