using Guppy.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Configurations
{
    public class DriverConfiguration
    {
        public Type DrivenType { get; private set; }
        public Type DriverType { get; private set; }

        public DriverConfiguration(Type drivenType, Type driverType)
        {
            if (!typeof(Driven).IsAssignableFrom(drivenType))
                throw new Exception("Unable to create driven configuration, invalid driven type!");
            else if (!typeof(Driver).IsAssignableFrom(driverType))
                throw new Exception("Unable to create driver configuration, invalid driver type!");

            this.DrivenType = drivenType;
            this.DriverType = driverType;
        }
    }
}
