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
    /// <typeparam name="TCreatable"></typeparam>
    public class CreatableFactory<TCreatable> : Factory<TCreatable>
        where TCreatable : Creatable
    {
        #region Private Fields
        private IPoolManager<TCreatable> _pools;
        #endregion

        #region Constructor
        public CreatableFactory(IPoolManager<TCreatable> pools, IServiceProvider provider) : base(pools, provider)
        {
            _pools = pools;
        }
        #endregion

        #region Build Methods
        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            return base.Build<T>(
                provider: provider,
                pool: pool,
                setup: creatable =>
                {
                    // Bind any required event handlers
                    creatable.Events.TryAdd<Creatable>("disposing", this.HandleInstanceDisposing);

                    setup?.Invoke(creatable);
                },
                create: creatable =>
                {
                    create?.Invoke(creatable);
                    creatable.TryCreate(provider);
                });
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
