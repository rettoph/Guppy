using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    /// <summary>
    /// Simple pooling class, used for
    /// recycling objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pool<T>
    {
        private Queue<T> _pool;

        public Pool()
        {
            _pool = new Queue<T>();
        }

        protected abstract T Create(IServiceProvider provider);

        public virtual void Put(T instance)
        {
            _pool.Enqueue(instance);
        }

        public virtual T Pull(IServiceProvider provider, Action<T> setup = null)
        {
            T child;

            if (_pool.Count == 0)
                child = this.Create(provider);
            else
                child = _pool.Dequeue();

            // Run the custom setup method if any
            setup?.Invoke(child);

            return child;
        }
    }
}
