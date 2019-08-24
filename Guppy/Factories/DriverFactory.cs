using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Loaders;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities;

namespace Guppy.Factories
{
    internal class DriverFactory : InitializableFactory<Driver>
    {
        #region Private Fields
        private DriverLoader _loader;
        private Dictionary<Type, Type[]> _types;
        #endregion

        #region Constructor
        public DriverFactory(DriverLoader loader, IPoolManager<Driver> pools, IServiceProvider provider) : base(pools, provider)
        {
            _types = new Dictionary<Type, Type[]>();
            _loader = loader;
        }
        #endregion

        /// <summary>
        /// Build all drivers for a specific input driven type and
        /// automatically add the drivers to the driven instance.
        /// </summary>
        /// <param name="driven"></param>
        public void BuildAll(Driven driven)
        {
            if (!_types.ContainsKey(driven.GetType()))
                _types[driven.GetType()] = _loader[driven.GetType()];

            _types[driven.GetType()].ForEach(driverType =>
            { // Iterate through all drivers bound to the given driven type...
                driven.Drivers.Add(this.Build(driverType, driver =>
                { // Build instances of the driverTypes & add them to the driven's driver collection
                    driver.SetParent(driven);
                }));
            });
        }
    }
}
