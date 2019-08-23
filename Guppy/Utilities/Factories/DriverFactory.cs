using Guppy.Collections;
using Guppy.Implementations;
using Guppy.Utilities.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Factories
{
    public class DriverFactory
    {
        private IServiceProvider _provider;
        private PooledFactory<Driver> _factory;
        private DriverLoader _loader;
        private PooledFactory<FrameableCollection<Driver>> _collectionFactory;

        public DriverFactory(IServiceProvider provider, PooledFactory<Driver> factory, PooledFactory<FrameableCollection<Driver>> collectionFactory, DriverLoader loader)
        {
            _provider = provider;
            _factory = factory;
            _collectionFactory = collectionFactory;
            _loader = loader;
        }

        public FrameableCollection<Driver> Pull(Driven driven)
        {
            var drivers = _collectionFactory.Pull();
            drivers.AddRange(_loader.GetValue(driven.GetType()).Select(t =>
            {
                return _factory.Pull<Driver>(t, d =>
                {
                    d.SetParent(driven);
                });
            }).ToArray());

            return drivers;
        }

        /// <summary>
        /// Return a collection of drivers back into their respectic pools
        /// and do the same with the frameable collection
        /// </summary>
        /// <param name="drivers"></param>
        public void Put(FrameableCollection<Driver> drivers)
        {
            drivers.Dispose();
            _collectionFactory.Put(drivers);
        }
    }
}
