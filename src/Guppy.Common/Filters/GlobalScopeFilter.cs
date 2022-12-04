using Guppy.Common;
using Guppy.Common.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Filters
{
    public class GlobalScopeFilter : SimpleFilter
    {
        public GlobalScopeFilter(Type implementationType) : base(implementationType)
        {
        }

        public override bool Invoke(IServiceProvider provider, Type implementationType)
        {
            var result = provider.GetRequiredService<Global>().Scope.Instance.GetHashCode() == provider.GetHashCode();

            return result;
        }
    }

    public class GlobalScopeFilter<TImplementation> : GlobalScopeFilter
    {
        public GlobalScopeFilter() : base(typeof(TImplementation))
        {
        }
    }
}
