﻿using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.Attributes;
using Guppy.Services;
using Guppy.Lists.Interfaces;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ServiceListServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddFactory<ServiceListService>(p => new ServiceListService());
            services.AddSingleton<ServiceListService>(autoBuild: true);

            services.AddFactory<ServiceList>(p => new ServiceList());
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

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
