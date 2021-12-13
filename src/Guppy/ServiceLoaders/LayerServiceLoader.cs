using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.DependencyInjection.Builders;
using DotNetUtils.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LayerServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
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

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
