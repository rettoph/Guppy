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
        public static IEnumerable<TAlias> GetImplementations<TAlias>(this IAliasProvider aliases, IServiceProvider provider)
            where TAlias : class
        {
            var implementations = aliases.GetImplementationTypes(typeof(TAlias), provider);
            foreach (Type implementation in implementations)
            {
                yield return (TAlias)provider.GetRequiredService(implementation);
            }
        }
    }
}
