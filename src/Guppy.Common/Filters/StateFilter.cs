using Guppy.Common.DependencyInjection.Interfaces;
using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Filters
{
    public class StateFilter<T> : SimpleFilter
    {
        public readonly T Value;

        public StateFilter(Type type, T value) : base(type)
        {
            this.Value = value;
        }

        public override bool Invoke(IStateProvider state, object service)
        {
            bool result = state.Matches(this.Value);
            return result;
        }
    }

    public class StateFilter<TService, TValue> : StateFilter<TValue>
    {
        public StateFilter(TValue value) : base(typeof(TService), value)
        {
        }
    }
}
