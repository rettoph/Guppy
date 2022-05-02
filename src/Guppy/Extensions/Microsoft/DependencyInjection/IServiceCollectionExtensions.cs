using Guppy.Providers;
using Minnow.System.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddActivated<T, TService>(this IServiceCollection services, bool singleton = false)
            where TService : class, T
        {
            var factory = ActivatorUtilitiesHelper.BuildFactory<TService>();

            return services.AddActivated<T, TService>(factory, singleton);
        }

        public static IServiceCollection AddActivated<T, TService>(this IServiceCollection services, Func<IServiceProvider, TService> factory, bool singleton = false)
            where TService : class, T
        {
            var ancestors = typeof(TService).GetAncestors<T>().Except(typeof(TService).Yield());

            if(singleton)
            {
                services.AddSingleton<TService>(p => p.GetRequiredService<ActivatedServiceProvider<T>>().TryActivate(factory));
            }
            else
            {
                services.AddScoped<TService>(p => p.GetRequiredService<ActivatedServiceProvider<T>>().TryActivate(factory));
            }

            foreach(Type ancestor in ancestors)
            {
                if(!services.Any(x => x.ServiceType == ancestor))
                {
                    if (singleton)
                    {
                        services.AddSingleton(ancestor, p => p.GetRequiredService<ActivatedServiceProvider<T>>().Instance!);
                    }
                    else
                    {
                        services.AddScoped(ancestor, p => p.GetRequiredService<ActivatedServiceProvider<T>>().Instance!);
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
