using Guppy.Implementations;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities
{
    public class DriverFactory
    {
        private PooledFactory _pooled;
        private DriverLoader _driverLoader;

        public DriverFactory(PooledFactory pooled, DriverLoader driverLoader)
        {
            _pooled = pooled;
            _driverLoader = driverLoader;
        }

        /// <summary>
        /// Return instances of drivers bound to the input driven type
        /// </summary>
        /// <param name="driven"></param>
        /// <returns></returns>
        public IEnumerable<Driver> GetDrivers(Driven driven)
        {
            // Get a list  of all driver types bound to the input driven type
            // Then create instances of them from the pool factory
            // Then update the parents in the fresh driver instances
            return _driverLoader.GetValue(driven.GetType()).Select(t =>
             {
                 return _pooled.Pull<Driver>(t, d =>
                 {
                     d.SetParent(driven);
                 });
             });
        }
    }
}
