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
        public void ConfigureServices(ServiceCollection services)
        {
            // Configure factories...
            services.AddFactory<LayerList>(p => new LayerList());
            services.AddFactory<OrderableList<Entity>>(p => new OrderableList<Entity>());

            // Configure services...
            services.AddScoped<LayerList>();
            services.AddTransient<OrderableList<Entity>>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
