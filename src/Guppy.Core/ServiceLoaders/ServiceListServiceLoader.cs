using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.Attributes;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ServiceListServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddFactory<ServiceList>(p => new ServiceList());
            services.AddSingleton<ServiceList>();

            services.AddConfiguration<IService>((s, p, d) =>
            {
                p.GetService<ServiceList>().TryAdd(s);
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
