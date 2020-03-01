using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Guppy.Factories
{
    public abstract class Factory<TBase>
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
        public TBase Build(IServiceProvider provider, Type type, Action<TBase> setup = null, Action<TBase> create = null)
        {
            return this.Build<TBase>(provider, type, setup, create);
        }

        public TBase Build(Type type, Action<TBase> setup = null, Action<TBase> create = null)
        {
            return this.Build<TBase>(type, setup, create);
        }
        public T Build<T>(Action<T> setup = null, Action<T> create = null)
            where T : TBase
        {
            return this.Build<T>(typeof(T), setup, create);
        }
        public T Build<T>(Type type, Action<T> setup = null, Action<T> create = null)
            where T : TBase
        {
            return this.Build<T>(_provider, type, setup, create);
        }
        public T Build<T>(IServiceProvider provider, Type type, Action<T> setup = null, Action<T> create = null)
            where T : TBase
        {
            ExceptionHelper.ValidateAssignableFrom<T>(type);

            return this.Build<T>(provider, _pools.GetOrCreate(type), setup, create);
        }
        protected virtual T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
            where T : TBase
        {
#if DEBUG
            this.logger.LogTrace(() => $"Factory<{pool.TargetType.Name}> => Building {typeof(TBase).Name}<{pool.TargetType.Name}> instance...");
#endif
            var creatable = (T)pool.Pull(t =>
            { // Define a custom create method within the pool...
                var c = (T)ActivatorUtilities.CreateInstance(provider, t);
                create?.Invoke(c);
                return c;
            });

            // Run the setup method if one was provided.
            setup?.Invoke(creatable);

            // Return the instance.
            return creatable;
        }
        #endregion
    }
}
