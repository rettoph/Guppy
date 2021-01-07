using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Guppy.Services
{
    /// <summary>
    /// Simple class used internally by Driven instances
    /// to create the required drivers.
    /// </summary>
    internal sealed class DriverService : Service
    {
        #region Private Fields
        private Dictionary<Type, ValidateEventDelegate<Driven, ServiceProvider>> _driverFilters;
        private Dictionary<Type, List<Type>> _drivenDrivers;

        private List<Type> _driverTypes;
        private List<Type> _drivenTypes;
        private ServiceProvider _provider;
        #endregion

        #region Lifecycle Methods 
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _drivenTypes = AssemblyHelper.Types.GetTypesAssignableFrom<Driven>().Where(t => !t.IsAbstract).ToList();
            _driverTypes = AssemblyHelper.Types.GetTypesAssignableFrom<Driver>().Where(t => !t.IsAbstract).ToList();

            _drivenDrivers = _drivenTypes.ToDictionary(
                keySelector: t => t,
                elementSelector: t => new List<Type>());

            _driverFilters = _driverTypes.ToDictionary(
                keySelector: t => t,
                elementSelector: t => default(ValidateEventDelegate<Driven, ServiceProvider>));
        }
        #endregion

        #region Helper Methods
        internal void AddFilter(Type driver, Func<Driven, ServiceProvider, Boolean> filter)
            => _driverTypes.GetTypesAssignableFrom(driver).ForEach(driverType => _driverFilters[driverType] += filter.ToValidateEventDelegate());

        internal void BindDriver(Type driven, Type driver)
            => _drivenTypes.GetTypesAssignableFrom(driven).ForEach(drivenType => _drivenDrivers[drivenType].Add(driver));

        internal void BuildDrivers(Driven driven, ServiceProvider provider, ref List<Driver> drivers)
        {
            foreach(Type driverType in _drivenDrivers[driven.GetType()])
                if (_driverFilters[driverType].Validate(driven, provider, true))
                {
                    var driver = (Driver)provider.GetService(driverType);
                    driver.TryInitialize(driven, provider);
                    drivers.Add(driver);
                }
        }

        internal void ReleaseDrivers(Driven driven, ref List<Driver> drivers)
        {
            drivers.ForEach(d =>
            {
                d.TryRelease(driven);
                _provider.GetServiceFactory(d.GetType()).Pools[d].TryReturn(d);
            });

            drivers.Clear();
        }
        #endregion
    }
}
