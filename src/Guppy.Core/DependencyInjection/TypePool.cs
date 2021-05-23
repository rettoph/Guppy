using Guppy.Extensions.System;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// Represents a pool of services.
    /// </summary>
    public sealed class TypePool
    {
        private Stack<Object> _pool;
        private UInt16 _poolSize;
        private UInt16 _maxPoolSize;
        private Type _type;

        public TypePool(Type type, ref UInt16 maxPoolSize)
        {
            _type = type;
            _maxPoolSize = maxPoolSize;
            _poolSize = 0;
            _pool = new Stack<Object>();
        }

        public Boolean Any()
            => _pool.Any();

        public Object Pull()
        {
            _poolSize--;

            return _pool.Pop();
        }

        /// <summary>
        /// If the internal pool is not yet at max capacity,
        /// return the recieved instance into the pool.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public Boolean TryReturn(Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(_type, instance.GetType());

            if (_poolSize < _maxPoolSize)
            {
                _pool.Push(instance);
                _poolSize++;
                return true;
            }

            return false;
        }

        public Int32 Count()
            => _pool.Count();
    }
}
