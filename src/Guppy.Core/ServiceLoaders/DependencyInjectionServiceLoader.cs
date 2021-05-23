using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceCollection = Guppy.DependencyInjection.ServiceCollection;
using ServiceProvider = Guppy.DependencyInjection.ServiceProvider;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class DependencyInjectionServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.RegisterTypeFactory<IServiceScopeFactory>(p => new ServiceScopeFactory(p));
            services.RegisterScoped<IServiceScopeFactory>();

            services.RegisterTypeFactory<ServiceScope>(p => new ServiceScope(p));
            services.RegisterScoped<ServiceScope>();

            services.RegisterTypeFactory<Settings>(p => new Settings());
            services.RegisterSingleton<Settings>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
