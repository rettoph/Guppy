using Guppy.Common;
using Guppy.Common.DependencyInjection.Interfaces;
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
        public GlobalScopeFilter(Type type) : base(type)
        {
        }

        public override bool Invoke(IServiceProvider provider, object service)
        {
            var result = provider.GetRequiredService<Global>().Scope.Instance.GetHashCode() == provider.GetHashCode();

            return result;
        }
    }

    public class GlobalScopeFilter<T> : GlobalScopeFilter
    {
        public GlobalScopeFilter() : base(typeof(T))
        {
        }
    }
}
