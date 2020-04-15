using Guppy.Attributes;
using Guppy.Collections;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LayerServiceCollection : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<LayerCollection>((p) => new LayerCollection());
            services.AddTransient<LayerEntityCollection>((p) => new LayerEntityCollection());
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
