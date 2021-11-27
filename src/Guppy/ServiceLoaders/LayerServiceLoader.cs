using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LayerServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            // Configure factories...
            services.RegisterTypeFactory<Layer>(p => new Layer());
            services.RegisterTypeFactory<LayerList>(p => new LayerList());
            services.RegisterTypeFactory<OrderableList<ILayerable>>(p => new OrderableList<ILayerable>());

            // Configure services...
            services.RegisterService<Layer>()
                .SetLifetime(ServiceLifetime.Transient);

            services.RegisterService<LayerList>()
                .SetLifetime(ServiceLifetime.Scoped);

            services.RegisterService<OrderableList<ILayerable>>()
                .SetLifetime(ServiceLifetime.Transient);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
