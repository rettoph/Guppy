using Guppy.Pooling;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities;
using Guppy.Pooling.Interfaces;
using Microsoft.Extensions.Logging;

namespace Guppy.Factories
{
    /// <summary>
    /// A factory that can create instances of Creatable objects.
    /// 
    /// This will automatically pool them and run TryCreate when
    /// they are first created.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    public class CreatableFactory<TBase> : Factory<TBase>
        where TBase : Creatable
    {
        #region Private Fields
        private IPoolManager<TBase> _pools;
        #endregion

        #region Constructor
        public CreatableFactory(IPoolManager<TBase> pools, IServiceProvider provider) : base(pools, provider)
        {
            _pools = pools;
        }
        #endregion

        #region Build Methods
        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null)
        {
            this.logger.LogTrace($"Factory<{pool.TargetType.Name}> => Building {typeof(TBase).Name}<{pool.TargetType.Name}> instance...");

            var instance = pool.Pull(t =>
            { // Define a custom create method within the pool...
                var i = ActivatorUtilities.CreateInstance(provider, t) as Creatable;
                i.TryCreate(provider);
                return i;
            }) as T;

            // Bind any required event handlers
            instance.Events.Add<Creatable>("disposing", this.HandleInstanceDisposing);

            // Run the setup method if one was provided.
            setup?.Invoke(instance);

            // Return the instance.
            return instance;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Automatically return any disposed instances back into the pool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void HandleInstanceDisposing(object sender, Creatable arg)
        {
            _pools.GetOrCreate(sender.GetType()).Put(sender);
        }
        #endregion
    }
}
