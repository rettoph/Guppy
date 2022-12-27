using Guppy.Common.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public class DefaultServiceFilter
    {
        public readonly IServiceFilter Instance;
        public readonly Type Type;

        public DefaultServiceFilter(IServiceFilter instance, Type? type = null)
        {
            this.Instance = instance;
            this.Type = type ?? instance.GetType();

            ThrowIf.Type.IsNotAssignableFrom(this.Type, instance.GetType());
        }
    }

    public class DefaultServiceFilter<T> : DefaultServiceFilter
    {
        public DefaultServiceFilter(IServiceFilter instance) : base(instance, typeof(T))
        {
        }
    }
}
