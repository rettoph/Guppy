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
            foreach(Type implementation in aliases.GetImplementationTypes(typeof(TAlias), provider))
            {
                yield return (TAlias)provider.GetRequiredService(implementation);
            }
        }
    }
}
