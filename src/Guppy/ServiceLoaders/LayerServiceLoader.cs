using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LayerServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            // Configure factories...
            services.RegisterTypeFactory<LayerList>(p => new LayerList());
            services.RegisterTypeFactory<OrderableList<ILayerable>>(p => new OrderableList<ILayerable>());

            // Configure services...
            services.RegisterScoped<LayerList>();
            services.RegisterTransient<OrderableList<ILayerable>>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
