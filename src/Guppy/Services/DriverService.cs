using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.Collections;
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
        private Dictionary<Type, ValidateEventDelegate<Driven, ServiceProvider>> _filters;
        private Dictionary<Type, List<Type>> _drivers;
        #endregion

        #region Lifecycle Methods 
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _drivers = AssemblyHelper.GetTypesAssignableFrom<Driven>().ToDictionary(
                keySelector: t => t,
                elementSelector: t => new List<Type>());

            _filters = AssemblyHelper.GetTypesAssignableFrom<Driver>().ToDictionary(
                keySelector: t => t,
                elementSelector: t => default(ValidateEventDelegate<Driven, ServiceProvider>));
        }
        #endregion

        #region Helper Methods
        internal void AddFilter(Type driver, ValidateEventDelegate<Driven, ServiceProvider> filter)
            => _filters[driver] += filter;

        internal void AddDriver(Type driven, Type driver)
            => _drivers[driven].Add(driver);

        internal Driver[] BuildDrivers(Driven driven, ServiceProvider provider)
        {
            var drivers = new List<Driver>();

            _drivers[driven.GetType()].ForEach(driverType =>
            {
                if (_filters[driverType]?.Validate(driven, provider) ?? true)
                {
                    var driver = (Driver)provider.GetService(driverType);
                    drivers.Add(driver);
                }
            });

            return drivers.ToArray();
        }
        #endregion
    }
}
