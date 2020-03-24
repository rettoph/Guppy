using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LayerServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<LayerCollection>();
            services.AddScoped<LayerEntityCollection>();

            AssemblyHelper.GetTypesAssignableFrom<Layer>().Where(l => l.IsClass && !l.IsAbstract).ForEach(t =>
            { // Auto register any Layer classes as a transient type.
                services.AddTransient(t);
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
