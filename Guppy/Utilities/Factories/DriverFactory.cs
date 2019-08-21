using Guppy.Collections;
using Guppy.Implementations;
using Guppy.Loaders;
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

        public DriverFactory(IServiceProvider provider, PooledFactory<Driver> factory, DriverLoader loader)
        {
            _provider = provider;
            _factory = factory;
            _loader = loader;
        }

        public FrameableCollection<Driver> Pull(Driven driven)
        {
            var drivers = new FrameableCollection<Driver>(_provider);
            drivers.AddRange(_loader.GetValue(driven.GetType()).Select(t =>
            {
                return _factory.Pull<Driver>(t, d =>
                {
                    d.SetParent(driven);
                });
            }).ToArray());

            return drivers;
        }
    }
}
