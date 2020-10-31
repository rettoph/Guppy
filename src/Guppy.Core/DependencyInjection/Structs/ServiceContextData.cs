using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Structs
{
    internal struct ServiceContextData
    {
        public String Name;
        public Type ServiceType;
        public Type Factory;
        public ServiceLifetime? Lifetime;
        public Int32 Priority;
        public Boolean AutoBuild;
    }
}
