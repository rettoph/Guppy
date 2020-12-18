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
    public sealed class ServicePool
    {
        private Stack<Object> _pool;
        private UInt16 _poolSize;
        private UInt16 _maxPoolSize;
        private Type _type;

        public ServicePool(Type type, ref UInt16 maxPoolSize)
        {
            _type = type;
            _maxPoolSize = maxPoolSize;
            _poolSize = 0;
            _pool = new Stack<Object>();
        }

        public Boolean Any()
            => _pool.Any();

        public Object Pull(Action<Type, Object> cacher = null)
        {
            _poolSize--;
            
            if(cacher == default)
                return _pool.Pop();
            else
                return _pool.Pop().Then(i => cacher(_type, i));
        }

        /// <summary>
        /// If the internal pool is not yet at max capacity,
        /// return the recieved instance into the pool.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public Boolean TryReturn(Object instance)
        {
            try
            {
                ExceptionHelper.ValidateAssignableFrom(_type, instance.GetType());

                if (_poolSize < _maxPoolSize)
                {
                    _pool.Push(instance);
                    _poolSize++;
                    return true;
                }
            }
            catch (Exception e)
            {
                // 
            }

            return false;
        }

        public Int32 Count()
            => _pool.Count();
    }
}
