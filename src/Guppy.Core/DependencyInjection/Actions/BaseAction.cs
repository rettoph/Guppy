using Guppy.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Actions
{
    public class BaseAction<TKey, TArgs> : IAction<TKey, TArgs>
    {
        private Action<Object, GuppyServiceProvider, TArgs> _method;
        private Func<IAction<TKey, TArgs>, TKey, Boolean> _filter;

        public TKey Key { get; private set; }
        public Int32 Order { get; private set; }

        internal BaseAction(
            TKey key, 
            Action<Object, GuppyServiceProvider, TArgs> method, 
            Int32 order = 0, 
            Func<IAction<TKey, TArgs>, TKey, Boolean> filter = default)
        {
            _method = method;
            _filter = filter ?? this.DefaultFilter;

            this.Key = key;
            this.Order = order;
        }

        public void Invoke(Object instance, GuppyServiceProvider provider, TArgs args)
            => _method(instance, provider, args);

        public virtual Boolean Filter(TKey key)
            => _filter(this, key);

        private Boolean DefaultFilter(IAction<TKey, TArgs> action, TKey key)
            => true;
    }
}
