using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.TypePools
{
    public class GenericTypePool
    {
        private Dictionary<Type, TypePool> _pools;
        private UInt16 _maxPoolSize;
        private Type _genericType;
        private TypePool _pool;

        public GenericTypePool(Type type, ref UInt16 maxPoolSize)
        {
            if (!type.IsGenericTypeDefinition)
                throw new ArgumentException($"Unable to create GenericTypePool; invalid Generic Type recieved => {type}");

            _maxPoolSize = maxPoolSize;
            _pools = new Dictionary<Type, TypePool>();
        }

        /// <summary>
        /// If the internal pool is not yet at max capacity,
        /// return the recieved instance into the pool.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public Boolean TryReturn(Object instance)
        {
            if(_pools.TryGetValue(instance.GetType(), out _pool))
                return _pool.TryReturn(instance);

            ExceptionHelper.ValidateAssignableFrom(_genericType, instance.GetType());

            _pools.Add(instance.GetType(), new TypePool(instance.GetType(), ref _maxPoolSize));
            return this.TryReturn(instance);
        }

        public Int32 Count()
            => _pool.Count();
    }
}
