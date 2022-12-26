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
        public static IEnumerable<TAlias> GetServices<TAlias>(this IAliasProvider aliases, IServiceProvider provider, object? configuration)
            where TAlias : class
        {
            var implementations = aliases.GetServiceTypes(typeof(TAlias), provider, configuration);
            foreach (Type implementation in implementations)
            {
                yield return (TAlias)provider.GetRequiredService(implementation);
            }
        }

        public static TAlias? GetService<TAlias>(this IAliasProvider aliases, IServiceProvider provider, object? configuration)
            where TAlias : class
        {
            var implementation = aliases.GetServiceTypes(typeof(TAlias), provider, configuration).LastOrDefault();

            if(implementation is null)
            {
                return null;
            }

            return (TAlias)provider.GetRequiredService(implementation);
        }
    }
}
