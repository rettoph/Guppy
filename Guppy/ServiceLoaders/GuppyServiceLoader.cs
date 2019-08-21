using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Configurations;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<GameConfiguration>();
            services.AddSingleton<Game>(p => p.GetService<GameConfiguration>().Instance);

            services.AddScoped<SceneConfiguration>();
            services.AddScoped<Scene>(p => p.GetService<SceneConfiguration>().Instance);

            services.AddSingleton<PooledFactory>();
            services.AddScoped<DriverFactory>();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
