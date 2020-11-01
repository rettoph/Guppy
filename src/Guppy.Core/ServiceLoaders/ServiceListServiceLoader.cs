using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Lists;
using Guppy.Attributes;
using Guppy.Services;
using Guppy.Lists.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ServiceListServiceLoader : IServiceLoader
    {
        public void ConfigureServices(GuppyServiceCollection services)
        {
            services.AddSingleton<ServiceListService>();
            services.AddSingleton<ServiceList>();

            services.AddConfiguration<IServiceList>((l, p, d) =>
            {
                if(d.Lifetime != ServiceLifetime.Transient)
                    p.GetService<ServiceListService>().TryAddList(l);
            });

            services.AddConfiguration<IService>((s, p, d) =>
            {
                p.GetService<ServiceListService>().TryAddService(s, p);
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
