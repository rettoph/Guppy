using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public abstract class ServiceFilter : IServiceFilter
    {
        public abstract Type Type { get; }

        public abstract bool Invoke(IServiceProvider provider);
    }

    public abstract class ServiceFilter<T> : ServiceFilter, IServiceFilter
    {
        public override Type Type { get; } = typeof(T);
    }
}
