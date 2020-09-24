using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Structs
{
    internal struct ServiceFactoryData
    {
        public Type Type;
        public Func<ServiceProvider, Object> Factory;
        public Int32 Priority;
    }
}
