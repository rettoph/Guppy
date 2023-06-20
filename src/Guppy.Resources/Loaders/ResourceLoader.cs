using Guppy.Loaders;
using Guppy.Resources.Providers;
using Guppy.Resources.Serialization.Json.Converters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Resources.Loaders
{
    internal sealed class ResourceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IResourcePackProvider, ResourcePackProvider>();
            services.AddSingleton<IResourceProvider, ResourceProvider>();
        }
    }
}
