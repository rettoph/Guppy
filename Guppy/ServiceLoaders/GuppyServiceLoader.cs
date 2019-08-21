using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Delegaters;
using Guppy.Utilities.Factories;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    public class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<EventDelegater>();

            services.AddSingleton<GameOptions>(p => p.GetService<IOptionsMonitor<GameOptions>>().CurrentValue);
            services.AddScoped<SceneOptions>(p => p.GetService<IOptionsMonitor<SceneOptions>>().Get(p.GetHashCode().ToString()));

            services.AddScoped<DriverFactory>();
            services.AddScoped<PoolFactory>();
            services.AddScoped(typeof(PooledFactory<>));

            services.AddSingleton<SceneCollection>();
            services.AddScoped<LayerCollection>();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
