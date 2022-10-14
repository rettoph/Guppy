using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    internal sealed class RuntimeServiceTypeFilter<T, TImplementation> : IServiceTypeFilter<T>, IFilter<IServiceProvider>
        where T : class
        where TImplementation : T
    {
        private Func<IServiceProvider, bool> _filter;

        public Type ImplementationType => typeof(TImplementation);

        public int Order { get; private set; }

        public RuntimeServiceTypeFilter(Func<IServiceProvider, bool> filter, int order)
        {
            _filter = filter;

            Order = order;
        }

        public bool Invoke(IServiceProvider arg)
        {
            return _filter(arg);
        }
    }
}
