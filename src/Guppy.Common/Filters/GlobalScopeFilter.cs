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
        private bool _invoked;

        public GlobalScopeFilter(Type implementationType) : base(implementationType)
        {
        }

        public override bool Invoke(IServiceProvider provider, Type implementationType)
        {
            if(_invoked)
            {
                return false;
            }

            _invoked = provider.GetRequiredService<Global>().Scope.Instance.GetHashCode() == provider.GetHashCode();

            return _invoked;
        }
    }

    public class SingletonFilter<TImplementation> : GlobalScopeFilter
    {
        public SingletonFilter() : base(typeof(TImplementation))
        {
        }
    }
}
