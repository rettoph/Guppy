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
        public T Build<T>(Type type, Action<T> setup = null)
            where T : TBase
        {
            ExceptionHelper.ValidateAssignableFrom<T>(type);

            return this.Build<T>(_provider, _pools.GetOrCreate(type), setup);
        }
        public TBase Build(Type type, Action<TBase> setup = null)
        {
            ExceptionHelper.ValidateAssignableFrom<TBase>(type);

            return this.Build<TBase>(_provider, _pools.GetOrCreate(type), setup);
        }
        public T Build<T>(Action<T> setup = null)
            where T : TBase
        {
            return this.Build<T>(_provider, _pools.GetOrCreate<T>(), setup);
        }
        protected abstract T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null)
            where T : TBase;
        #endregion
    }
}
