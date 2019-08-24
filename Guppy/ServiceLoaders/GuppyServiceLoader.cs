using Guppy.Attributes;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Pooling;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Utilities.Options;
using Guppy.Collections;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<EventDelegater>();

            services.AddSingleton<GlobalOptions>();
            services.AddScoped<ScopeOptions>();

            services.AddScoped(typeof(IPoolManager), typeof(PoolManager));
            services.AddScoped(typeof(IPoolManager<>), typeof(PoolManager<>));
            services.AddScoped(typeof(IPool<>), typeof(Pool<>));
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
        }
    }
}
