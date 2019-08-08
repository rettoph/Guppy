using Guppy.Interfaces;
using Guppy.Utilities.Pools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Configurations
{
    public class DriverConfiguration
    {
        public Type Driven { get; set; }
        public Type Driver { get; set; }
        public Pool<IDriver> Pool { get; set; }
    }
}
