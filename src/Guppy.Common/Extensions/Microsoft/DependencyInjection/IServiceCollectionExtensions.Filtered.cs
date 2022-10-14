using Guppy;
using Guppy.Common;
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
            return services.RemoveBy(x => x.ServiceType == typeof(T) || x.ServiceType == typeof(Filtered<T>), out _)
                .AddSingleton<IServiceTypeFilter<T>>(new RuntimeServiceTypeFilter<T, TImplementation>(filter, order))
                .AddSingleton<Filtered<T>>()
                .AddTransient<T>(ImplementationFactory<T>);
        }

        private static T ImplementationFactory<T>(IServiceProvider provider)
            where T : class
        {
            var implementationType = provider.GetRequiredService<Filtered<T>>().GetImplementationType(provider) ?? throw new Exception();
            return  provider.GetService(implementationType) as T ?? throw new Exception();
        }
    }
}
