using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Service loader used to automatically register all
    /// game types into the service collection.
    /// </summary>
    [IsServiceLoader]
    internal sealed class LayerServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DrivenFactory<Layer>>();
            services.AddScoped<LayerCollection>();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
        }
    }
}
