using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Loaders;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Factories
{
    internal class DriverFactory : Factory<Driver>
    {
        #region Private Fields
        private DriverLoader _loader;
        private Dictionary<Type, Type[]> _types;
        private IServiceProvider _provider;
        #endregion

        #region Constructor
        public DriverFactory(DriverLoader loader, IPoolManager<Driver> pools, IServiceProvider provider) : base(pools, provider)
        {
            _types = new Dictionary<Type, Type[]>();
            _loader = loader;
            _provider = provider;
        }
        #endregion

        /// <summary>
        /// Build all drivers for a specific input driven type and
        /// automatically add the drivers to the driven instance.
        /// </summary>
        /// <param name="driven"></param>
        public FrameableCollection<Driver> BuildAll(Driven driven)
        {
            if (!_types.ContainsKey(driven.GetType()))
                _types[driven.GetType()] = _loader[driven.GetType()];

            var collection = _provider.GetRequiredService<FrameableCollection<Driver>>();

            // Add all the driver instances to the newly created collection
            collection.AddRange(_types[driven.GetType()].Select(driverType =>
            { // Iterate through all drivers bound to the given driven type...
                return this.Build(driverType, driver =>
                { // Build instances of the driverTypes & add them to the driven's driver collection
                    driver.SetParent(driven);
                });
            }));

            return collection;
        }

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            var driver = pool.Pull(p =>
            {
                var d = ActivatorUtilities.CreateInstance(provider, pool.TargetType) as T;
                create?.Invoke(d);
                return d;
            }) as T;

            setup?.Invoke(driver);

            return driver;
        }
    }
}
