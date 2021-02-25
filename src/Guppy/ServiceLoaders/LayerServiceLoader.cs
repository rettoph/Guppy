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
            services.AddFactory<LayerList>(p => new LayerList());
            services.AddFactory<OrderableList<IEntity>>(p => new OrderableList<IEntity>());

            // Configure services...
            services.AddScoped<LayerList>();
            services.AddTransient<OrderableList<IEntity>>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
