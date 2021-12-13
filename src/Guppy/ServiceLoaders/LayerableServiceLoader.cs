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
    internal sealed class LayerableServiceCollection : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterService<LayerableList>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<LayerableList>();
                });

            services.RegisterSetup<ILayerable>()
                .SetMethod((l, p, c) =>
                {
                    p.GetService<LayerableList>().TryAdd(l);
                });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
