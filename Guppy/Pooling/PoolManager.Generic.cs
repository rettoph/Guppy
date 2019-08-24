using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Pooling
{
    /// <summary>
    /// Represents a types pool manager that contains a collection
    /// of pools belonging to types that extends the inputed Generic
    /// type. 
    /// 
    /// This creates smaller segmented dictionaries for improved lookup
    /// times.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    internal sealed class PoolManager<TBase> : IPoolManager<TBase>
    {
        #region Private Fields
        private IPoolManager _poolManager;
        private Dictionary<Type, IPool> _pools;
        #endregion

        #region Constructor
        public PoolManager(IPoolManager poolManager)
        {
            _poolManager = poolManager;
            _pools = new Dictionary<Type, IPool>();
        }
        #endregion

        #region IPoolManager Implementation
        public IPool GetOrCreate<T>() 
            where T : TBase
        {
            return this.GetOrCreate(typeof(T));
        }

        public IPool GetOrCreate(Type type)
        {
            ExceptionHelper.ValidateAssignableFrom<TBase>(type);

            if (!_pools.ContainsKey(type))
                _pools[type] = _poolManager.GetOrCreate(type);

            return _pools[type];
        }
        #endregion
    }
}
