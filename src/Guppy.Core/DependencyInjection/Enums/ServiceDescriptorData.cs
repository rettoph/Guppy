using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Enums
{
    internal struct ServiceDescriptorData
    {
        public String Name;
        public Type Factory;
        public ServiceLifetime Lifetime;
        public Type CacheType;
        public Int32 Priority;
    }
}
