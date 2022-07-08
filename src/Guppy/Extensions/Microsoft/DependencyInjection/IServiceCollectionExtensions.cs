using Guppy.Common.Helpers;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFaceted<T, TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class, T
            where TImplementation : class, TService
        {
            var factory = ActivatorUtilitiesHelper.BuildFactory<TImplementation>();

            return services.AddFaceted<T, TService>(factory, lifetime);
        }

        public static IServiceCollection AddFaceted<T, TService>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class, T
        {
            var factory = ActivatorUtilitiesHelper.BuildFactory<TService>();

            return services.AddFaceted<T, TService>(factory, lifetime);
        }

        public static IServiceCollection AddFaceted<T, TService>(this IServiceCollection services, Func<IServiceProvider, TService> factory, ServiceLifetime lifetime)
            where TService : class, T
        {
            var ancestors = typeof(TService).GetAncestors<T>().Except(typeof(TService).Yield());

            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<Faceted<T>>();
                    services.AddSingleton<TService>(p => p.GetRequiredService<Faceted<T>>().Activate(factory));
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<Faceted<T>>();
                    services.AddScoped<TService>(p => p.GetRequiredService<Faceted<T>>().Activate(factory));
                    break;
                case ServiceLifetime.Transient:
                    throw new NotImplementedException();
            }

            foreach(Type ancestor in ancestors)
            {
                if(!services.Any(x => x.ServiceType == ancestor))
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(ancestor, p => p.GetRequiredService<Faceted<T>>().Instance!);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(ancestor, p => p.GetRequiredService<Faceted<T>>().Instance!);
                            break;
                        case ServiceLifetime.Transient:
                            throw new NotImplementedException();
                    }
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
