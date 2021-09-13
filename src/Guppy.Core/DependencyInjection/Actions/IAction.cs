using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Actions
{
    public interface IAction<TKey, TArgs>
    {
        public TKey Key { get; }

        public Int32 Order { get; }

        void Invoke(Object instance, GuppyServiceProvider provider, TArgs args);

        Boolean Filter(TKey key);
    }
}
