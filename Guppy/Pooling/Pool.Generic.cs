using Guppy.Pooling.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Pooling
{
    internal sealed class Pool<T> : IPool<T>
    {
        private IPool _pool;

        public Pool(IPoolManager pools)
        {
            _pool = pools.GetOrCreate(typeof(T));
        }

        public int Count()
        {
            return _pool.Count();
        }

        public T Pull(Func<Type, T> factory)
        {
            return (T)_pool.Pull(t => factory(t));
        }

        public void Put(T instance)
        {
            _pool.Put(instance);
        }
    }
}
