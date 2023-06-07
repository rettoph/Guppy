using Guppy.Common.DependencyInjection.Interfaces;
using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Filters
{
    public class ServiceFilter<T> : IServiceFilter
    {
        public readonly Type Type;

        public readonly T Value;

        public ServiceFilter(Type type, T value)
        {
            this.Value = value;
            this.Type = type;
        }

        public virtual bool AppliesTo(Type type)
        {
            var result = this.Type.IsAssignableFrom(type);

            return result;
        }

        public virtual bool Invoke(IStateProvider state)
        {
            bool result = state.Matches(this.Value);
            return result;
        }
    }

    public class ServiceFilter<TService, TValue> : ServiceFilter<TValue>
    {
        public ServiceFilter(TValue value) : base(typeof(TService), value)
        {
        }
    }
}
