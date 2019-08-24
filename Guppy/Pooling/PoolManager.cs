using Guppy.Factories;
using Guppy.Pooling.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Guppy.Pooling
{
    /// <summary>
    /// Represents a collection of all pools currently
    /// created within the scope.
    /// 
    /// This should be used to get or create pool instances.
    /// </summary>
    internal sealed class PoolManager : IPoolManager
    {
        #region Private Fields
        private Dictionary<Type, IPool> _pools;
        private ILogger _logger;
        private IServiceProvider _provider;
        #endregion

        #region Constructor
        public PoolManager(ILogger logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;

            _pools = new Dictionary<Type, IPool>();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Get an existing pool for the specified type or create a new one.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IPool GetOrCreate(Type type)
        {
            if (!_pools.ContainsKey(type))
            {
                _logger.LogTrace($"Creating new Pool<{type.Name}> instance...");
                _pools[type] = ActivatorUtilities.CreateInstance<Pool>(_provider, type);
            }

            return _pools[type];
        }

        public IPool GetOrCreate<T>()
        {
            return this.GetOrCreate(typeof(T));
        }
        #endregion
    }
}
