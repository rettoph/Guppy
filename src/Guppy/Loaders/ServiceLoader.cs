using Guppy.Attributes;
using Guppy.Common;
using Guppy.Loaders;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Loaders
{
    [AutoLoad]
    internal sealed class ServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGuppyProvider, GuppyProvider>()
                    .AddSingleton(typeof(IGlobal<>), typeof(Global<>))
                    .AddTransient(typeof(Lazy<>), typeof(Lazier<>))
                    .AddTransient(typeof(IScoped<>), typeof(Scoped<>))
                    .AddTransient(typeof(IFiltered<>), typeof(Filtered<>))
                    .AddScoped<IBus, Bus>();
        }
    }
}
