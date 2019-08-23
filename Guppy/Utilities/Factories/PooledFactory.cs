using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Factories
{
    public sealed class PooledFactory<TBase>
        where TBase : class
    {
        #region Private Fields
        private ILogger _logger;
        private PoolFactory _poolFactory;
        #endregion

        #region Constructor
        public PooledFactory(ILogger logger, PoolFactory poolFactory)
        {
            _logger = logger;
            _poolFactory = poolFactory;
        }
        #endregion

        #region Helper Methods
        public TBase Pull(Action<TBase> setup = null)
        {
            return this.Pull(typeof(TBase), setup);
        }
        public TBase Pull(Type type, Action<TBase> setup = null)
        {
            var instance = _poolFactory.GetOrCreatePool(type).Pull<TBase>(setup);

            return instance;
        }
        public TInstance Pull<TInstance>(Action<TInstance> setup = null)
            where TInstance : class, TBase
        {
            _logger.LogTrace($"Pulling {typeof(TBase).Name}<{typeof(TInstance).Name}> instance...");
            var instance = _poolFactory.GetOrCreatePool(typeof(TInstance)).Pull<TInstance>(setup);

            return instance;
        }
        public TInstance Pull<TInstance>(Type type, Action<TInstance> setup = null)
            where TInstance : class, TBase
        {
            _logger.LogTrace($"Pulling {typeof(TBase).Name}<{type.Name}> instance...");
            var instance = _poolFactory.GetOrCreatePool(type).Pull<TInstance>(setup);

            return instance;
        }
        #endregion

        #region Put Methods
        /// <summary>
        /// Manually return an instance back into its respective pool.
        /// </summary>
        /// <param name="instance"></param>
        public void Put(TBase instance)
        {
            _poolFactory.GetOrCreatePool(instance.GetType()).Put(instance);
        }
        #endregion
    }
}
