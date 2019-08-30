using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public abstract class Factory<TBase>
        where TBase : class
    {
        #region Private Fields
        private IPoolManager<TBase> _pools;
        private IServiceProvider _provider;
        #endregion

        #region Protected Fields
        protected ILogger logger;
        #endregion

        #region Constructor
        public Factory(IPoolManager<TBase> pools, IServiceProvider provider)
        {
            _provider = provider;
            _pools = pools;

            this.logger = _provider.GetRequiredService<ILogger>();
        }
        #endregion

        #region Create Methods
        public T Build<T>(Type type, Action<T> setup = null, Action<T> create = null)
            where T : class, TBase
        {
            ExceptionHelper.ValidateAssignableFrom<T>(type);

            return this.Build<T>(_provider, _pools.GetOrCreate(type), setup, create);
        }
        public TBase Build(Type type, Action<TBase> setup = null, Action<TBase> create = null)
        {
            ExceptionHelper.ValidateAssignableFrom<TBase>(type);

            return this.Build<TBase>(_provider, _pools.GetOrCreate(type), setup, create);
        }
        public T Build<T>(Action<T> setup = null, Action<T> create = null)
            where T : class, TBase
        {
            return this.Build<T>(_provider, _pools.GetOrCreate<T>(), setup, create);
        }
        protected virtual T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
            where T : class, TBase
        {
            this.logger.LogTrace($"Factory<{pool.TargetType.Name}> => Building {typeof(TBase).Name}<{pool.TargetType.Name}> instance...");

            var creatable = pool.Pull(t =>
            { // Define a custom create method within the pool...
                var c = ActivatorUtilities.CreateInstance(provider, t) as T;
                create?.Invoke(c);
                return c;
            }) as T;

            // Run the setup method if one was provided.
            setup?.Invoke(creatable);

            // Return the instance.
            return creatable;
        }
        #endregion
    }
}
