using Guppy;
using Guppy.Common;
using Guppy.Common.Providers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFilter<T, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, bool> filter, 
            int order)
                where T : class
                where TImplementation : T
        {
            // Add the filter instance
            services.AddSingleton<IServiceTypeFilter<T>>(new RuntimeServiceTypeFilter<T, TImplementation>(filter, order));

            // Check to see if a filtered provider exists. If it doesn't, add one.
            if(!services.Any(x => x.ServiceType == typeof(FilteredProvider<T>)))
            {
                services.AddScoped<FilteredProvider<T>>();
            }

            // Check to see if a descriptor for the service type exists. If it doesn't, add one.
            if (!services.Any(x => x.ServiceType == typeof(T)))
            {
                services.Add(ServiceDescriptor.Describe(typeof(T), ImplementationFactory<T>, ServiceLifetime.Scoped));
            }

            // Check to see if a descriptor for the service type enumerable exists. If it doesn't, add one.
            if (!services.Any(x => x.ServiceType == typeof(IEnumerable<T>)))
            {
                services.Add(ServiceDescriptor.Describe(typeof(IEnumerable<T>), EnumerableImplementationFactory<T>, ServiceLifetime.Scoped));
            }

            return services;
        }

        private static object ImplementationFactory<T>(IServiceProvider provider)
            where T : class
        {
            var implementationTypes = provider.GetRequiredService<FilteredProvider<T>>().GetImplementationTypes(provider) ?? throw new Exception();
            
            if(implementationTypes.Length == 0)
            {
                throw new Exception();
            }

            return provider.GetRequiredService(implementationTypes[0]);
        }

        private static object EnumerableImplementationFactory<T>(IServiceProvider provider)
            where T : class
        {
            var implementationTypes = provider.GetRequiredService<FilteredProvider<T>>().GetImplementationTypes(provider) ?? throw new Exception();
            return implementationTypes.Select(x => provider.GetRequiredService(x) as T);
        }
    }
}
