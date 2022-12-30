using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public static class IAliasProviderExtensions
    {
        public static IEnumerable<TAlias> GetServices<TAlias>(this IAliasProvider aliases, IServiceProvider provider)
            where TAlias : class
        {
            var implementations = aliases.GetServiceTypes(typeof(TAlias), provider);
            foreach (Type implementation in implementations)
            {
                var instance = provider.GetRequiredService(implementation);

                if(instance is TAlias casted)
                {
                    yield return casted;
                }
            }
        }

        public static TAlias? GetService<TAlias>(this IAliasProvider aliases, IServiceProvider provider)
            where TAlias : class
        {
            var implementation = aliases.GetServiceTypes(typeof(TAlias), provider).LastOrDefault();

            if(implementation is null)
            {
                return null;
            }

            return (TAlias)provider.GetRequiredService(implementation);
        }
    }
}
