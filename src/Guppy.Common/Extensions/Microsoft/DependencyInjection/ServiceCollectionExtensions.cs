﻿using Guppy.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActivated<T, TService>(this IServiceCollection services)
            where TService : class, T
        {
            var factory = ActivatorUtilities.CreateFactory(typeof(TService), Array.Empty<Type>());
            var args = Array.Empty<object>();

            return services.AddActivated<T, TService>(p => (TService)factory(p, args));
        }

        public static IServiceCollection AddActivated<T, TService>(this IServiceCollection services, Func<IServiceProvider, TService> factory)
            where TService : class, T
        {
            var ancestors = typeof(TService).GetAncestors<T>().Except(typeof(TService).Yield());

            services.AddScoped<TService>(p => p.GetRequiredService<ActivatedServiceProvider<T>>().TryActivate(factory));

            foreach(Type ancestor in ancestors)
            {
                if(!services.Any(x => x.ServiceType == ancestor))
                {
                    services.AddScoped(ancestor, p => p.GetRequiredService<ActivatedServiceProvider<T>>().Instance!);
                }
            }

            return services;
        }

        public static IServiceCollection RemoveBy(this IServiceCollection services, Func<ServiceDescriptor, bool> predicate, out ServiceDescriptor[] removed)
        {
            removed = services.Where(predicate).ToArray();

            foreach(ServiceDescriptor remove in removed)
            {
                services.Remove(remove);
            }

            return services;
        }
    }
}
