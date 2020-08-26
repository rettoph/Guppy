using Guppy.Attributes;
using Guppy.Collections;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LayerServiceCollection : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            // Configure factories...
            services.AddFactory<LayerCollection>(p => new LayerCollection());
            services.AddFactory<LayerEntityCollection>(p => new LayerEntityCollection());

            // Configure services...
            services.AddScoped<LayerCollection>();
            services.AddTransient<LayerEntityCollection>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
