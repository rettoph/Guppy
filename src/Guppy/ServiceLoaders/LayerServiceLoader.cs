using Guppy.Attributes;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Lists;
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
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<Layer>();
                });

            services.RegisterService<LayerList>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<LayerList>();
                });

            services.RegisterService<OrderableList<ILayerable>>()
                .SetLifetime(ServiceLifetime.Transient)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<OrderableList<ILayerable>>();
                });
        }
    }
}
