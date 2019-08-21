using Guppy.Implementations;
using Guppy.Loaders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Factories
{
    public class DriverFactory : PooledFactory<Driver>
    {
        private DriverLoader _driverLoader;

        public DriverFactory(DriverLoader driverLoader, ILogger logger, PoolFactory pooled) : base(logger, pooled)
        {
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
                return this.Pull<Driver>(t, d =>
                {
                    d.SetParent(driven);
                });
            }).ToArray();
        }
    }
}
