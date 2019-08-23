using Guppy.Factories;
using Guppy.Pooling.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Pooling
{
    /// <summary>
    /// Represents a collection of all pools currently
    /// created within the scope.
    /// 
    /// This should be used to get or create pool instances.
    /// </summary>
    public class PoolManager
    {
        #region Private Fields
        private Dictionary<Type, IPool> _pools;
        private IServiceProvider _provider;
        #endregion

        #region Constructor
        public PoolManager(IServiceProvider provider)
        {
            _provider = provider;

            _pools = new Dictionary<Type, IPool>();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Get an existing pool for the specified type or create a new one.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public virtual IPool GetOrCreate(Type targetType)
        {
            if (_pools.ContainsKey(targetType))
                return _pools[targetType];
            else
            {
                _pools[targetType] = new Pool(targetType);
                return _pools[targetType];
            }
        }
        #endregion
    }
}
