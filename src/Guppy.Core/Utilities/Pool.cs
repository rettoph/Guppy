using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities
{
    public class Pool<T> : IPool<T>
        where T : class
    {
        private Stack<T> _pool;
        private UInt16 _poolSize;
        private UInt16 _maxPoolSize;

        public Pool(ref UInt16 maxPoolSize)
        {
            _maxPoolSize = maxPoolSize;
            _poolSize = 0;
            _pool = new Stack<T>();
        }

        /// <inheritdoc />
        public virtual Boolean Any()
            => _pool.Any();

        /// <inheritdoc />
        public virtual Boolean TryPull(out T instance)
        {
            if (this.Any())
            {
                instance = _pool.Pop();

                _poolSize--;

                return true;
            }

            instance = default;
            return false;
        }

        /// <inheritdoc />
        public virtual Boolean TryReturn(T instance)
        {
            if (_poolSize < _maxPoolSize)
            {
                _pool.Push(instance);
                _poolSize++;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual Int32 Count()
            => _poolSize;
    }
}
