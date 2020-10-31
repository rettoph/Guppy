using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Structs
{
    internal struct ServiceFactoryData
    {
        public Type Type => this.Descriptor.ImplementationType ?? this.Descriptor.ServiceType;
        public ServiceDescriptor Descriptor;
        public Int32 Priority;
    }
}
