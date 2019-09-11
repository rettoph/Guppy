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
using Guppy.Extensions.Collection;
using System.Linq;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(DrivenFactory<>));
            services.AddScoped(typeof(OrderableCollection<>));

            services.AddTransient<EventDelegater>();

            services.AddSingleton<GlobalOptions>();
            services.AddScoped<ScopeOptions>();

            services.AddScoped(typeof(IPoolManager), typeof(PoolManager));
            services.AddScoped(typeof(IPoolManager<>), typeof(PoolManager<>));
            services.AddScoped(typeof(IPool<>), typeof(Pool<>));

            // Register all specified services based on the IsServiceAttribute
            AssemblyHelper.GetTypesWithAttribute<Object, IsServiceAttribute>().ForEach(t =>
            {
                t.GetCustomAttributes(true).Where(attr => attr is IsServiceAttribute).Select(attr => attr as IsServiceAttribute).ForEach(attr =>
                {
                    services.Add(new ServiceDescriptor(t, attr.Lifetime));
                });
            });
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
        }
    }
}
