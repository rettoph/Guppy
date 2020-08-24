using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Enums
{
    internal struct ServiceConfigurationData
    {
        public String Service;
        public Action<ServiceProvider, Object> Configuration;
        public Int32 Order;
        public Type Assignable;
    }
}
