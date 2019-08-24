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
        #endregion

        #region Constructors
        public DrivenFactory(IPoolManager<TDriven> pools, IServiceProvider provider) : base(pools, provider)
        {
            _driverFactory = provider.GetService<DriverFactory>();
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null)
        {
            var instance = base.Build<T>(provider, pool, driven =>
            {
                // Build drivers for the type...
                _driverFactory.BuildAll(driven);

                // Run the custom setup if there is any
                setup?.Invoke(driven);
            });

            return instance;
        }
    }
}
