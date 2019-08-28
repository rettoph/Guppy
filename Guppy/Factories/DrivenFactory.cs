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

        protected override void Create<T>(T driven)
        {
            base.Create(driven);

            // Build custom drivers for the new instance if needed.
            _driverFactory.BuildAll(driven);
        }
    }
}
