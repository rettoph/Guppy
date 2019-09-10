using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Loaders;
using Guppy.Pooling.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Factories
{
    public class DrivenFactory<TDriven> : InitializableFactory<TDriven>
        where TDriven : Driven
    {
        #region Private Fields
        private DriverLoader _drivers;
        #endregion

        #region Constructors
        public DrivenFactory(DriverLoader drivers, IPoolManager<TDriven> pools, IServiceProvider provider) : base(pools, provider)
        {
            _drivers = drivers;
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            return base.Build<T>(
                provider: provider,
                pool: pool,
                setup: setup,
                create: driven =>
                {
                    // Create driver instances as defined in the drivers loader
                    driven.drivers = _drivers[driven.GetType()]
                        .Select(t => ActivatorUtilities.CreateInstance(provider, t, driven) as Driver)
                        .ToArray();

                    create?.Invoke(driven);
                });

        }
    }
}
