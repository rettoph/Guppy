using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
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
            services.AddFactory<IServiceScopeFactory>(p => new ServiceScopeFactory(p));
            services.AddScoped<IServiceScopeFactory>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
