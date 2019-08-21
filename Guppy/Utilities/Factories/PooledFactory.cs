using Guppy.Implementations;
using Guppy.Loaders;
using Guppy.Utilities.Pools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Factories
{
    /// <summary>
    /// Internal class that contains instances of all
    /// registered pools within the InitializablePoolLoader.
    /// 
    /// This can be used to easily created scoped instances of 
    /// initializable objects.
    /// </summary>
    public class PooledFactory
    {
        #region Private Fields
        private IServiceProvider _provider;
        private Dictionary<Type, Pool> _pools;
        private PoolLoader _poolLoader;
        #endregion

        #region Constructors
        public PooledFactory(PoolLoader poolLoader, IServiceProvider provider)
        {
            _poolLoader = poolLoader;
            _provider = provider;

            _pools = new Dictionary<Type, Pool>();
        }
        #endregion

        #region Helper Methods
        public Object Pull(Type type, Action<Object> setup = null)
        {
            return this.GetOrCreatePool(type).Pull(_provider, setup);
        }
        public T Pull<T>(Action<T> setup = null)
            where T : class
        {
            return this.GetOrCreatePool(typeof(T)).Pull<T>(_provider, setup);
        }
        public T Pull<T>(Type type, Action<T> setup = null)
            where T : class
        {
            return this.GetOrCreatePool(type).Pull<T>(_provider, setup);
        }

        private Pool GetOrCreatePool(Type type)
        {
            if (!_pools.ContainsKey(type))
            {
                // If a pool does not alread exist for this type... create it.
                var pool = ActivatorUtilities.CreateInstance(_provider, _poolLoader.GetValue(type), type) as Pool;
                pool.TryCreate(_provider);

                _pools[type] = pool;
            }

            return _pools[type];
        }
        #endregion
    }
}
