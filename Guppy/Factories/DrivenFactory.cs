using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.Pooling.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Factories
{
    public class DrivenFactory<TDriven> : InitializableFactory<TDriven>
        where TDriven : IDriven
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
                    var drivers = new FrameableCollection<IDriver>(provider);
                    _drivers[driven.GetType().IsGenericType ? driven.GetType().GetGenericTypeDefinition() : driven.GetType()]
                        .Select(t => ActivatorUtilities.CreateInstance(provider, t, driven) as IDriver)
                        .ForEach(d => drivers.Add(d));
                    // Create driver instances as defined in the drivers loader
                    driven.Drivers = drivers;

                    create?.Invoke(driven);
                });

        }
    }
}
