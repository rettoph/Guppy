using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Actions
{
    public class BaseAction<TKey, TArgs> : IAction<TKey, TArgs>
    {
        private Action<Object, ServiceProvider, TArgs> _method;

        public TKey Key { get; private set; }
        public Int32 Order { get; private set; }

        internal BaseAction(TKey key, Action<Object, ServiceProvider, TArgs> method, Int32 order = 0)
        {
            _method = method;

            this.Key = key;
            this.Order = order;
        }

        public void Invoke(Object instance, ServiceProvider provider, TArgs args)
            => _method(instance, provider, args);
    }
}
