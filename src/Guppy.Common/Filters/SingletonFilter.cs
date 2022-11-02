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
    public class SingletonFilter : SimpleFilter
    {
        private bool _invoked;

        public SingletonFilter(Type implementationType) : base(implementationType)
        {
        }

        public override bool Invoke(IServiceProvider provider, Type implementationType)
        {
            if(_invoked)
            {
                return false;
            }

            _invoked = true;

            return true;
        }
    }

    public class SingletonFilter<TImplementation> : SingletonFilter
    {
        public SingletonFilter() : base(typeof(TImplementation))
        {
        }
    }
}
