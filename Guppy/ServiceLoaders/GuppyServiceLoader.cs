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

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    public class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<EventDelegater>();

            services.AddScoped<PoolManager>();
            services.AddScoped(typeof(IPool<>), typeof(Pool<>));
            services.AddScoped(typeof(CreatableFactory<>));
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
        }
    }
}
