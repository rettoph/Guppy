using Guppy.Pooling;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities;

namespace Guppy.Factories
{
    /// <summary>
    /// A factory that can create instances of Creatable objects.
    /// 
    /// This will automatically pool them and run TryCreate when
    /// they are first created.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    public class CreatableFactory<TBase>
        where TBase : Creatable
    {
        #region Private Fields
        private PoolManager _pools;
        #endregion

        #region Protected Fields
        protected IServiceProvider provider { get; private set; }
        #endregion

        #region Constructor
        public CreatableFactory(IServiceProvider provider)
        {
            _pools = provider.GetService<PoolManager>();

            this.provider = provider;
        }
        #endregion

        #region Create Methods
        public TBase Build(Type type, Action<TBase> setup = null)
        {
            return this.Build<TBase>(type, setup);
        }
        public virtual T Build<T>(Action<TBase> setup = null)
            where T : TBase
        {
            return this.Build<T>(typeof(T), setup);
        }
        protected virtual T Build<T>(Type type, Action<T> setup = null)
            where T : TBase
        {
            ExceptionHelper.ValidateAssignableFrom<T>(type);

            var instance = _pools.GetOrCreate(type).Pull(t =>
            { // Define a custom create method within the pool...
                var i = ActivatorUtilities.CreateInstance(this.provider, t) as Creatable;
                i.TryCreate(this.provider);
                return i;
            }) as T;

            // Run the setup method if one was provided.
            setup?.Invoke(instance);

            // Return the instance.
            return instance;
        }
        #endregion
    }
}
