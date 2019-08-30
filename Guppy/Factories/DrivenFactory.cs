using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Pooling.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Factories
{
    public class DrivenFactory<TDriven> : InitializableFactory<TDriven>
        where TDriven : Driven
    {
        #region Private Fields
        private DriverFactory _driverFactory;
        private IPoolManager<TDriven> _pools;
        #endregion

        #region Constructors
        public DrivenFactory(IPoolManager<TDriven> pools, IServiceProvider provider) : base(pools, provider)
        {
            _driverFactory = provider.GetService<DriverFactory>();
            _pools = pools;
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            return base.Build<T>(
                provider: provider,
                pool: pool,
                setup: driven =>
                {
                    // Bind any required event handlers
                    driven.Events.Add<Creatable>("disposing", this.HandleInstanceDisposing);

                    setup?.Invoke(driven);
                },
                create: driven =>
                {
                    // Build custom drivers for the new instance if needed.
                    driven.drivers = _driverFactory.BuildAll(driven);

                    create?.Invoke(driven);
                });

        }

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
