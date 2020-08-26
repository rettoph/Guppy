using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Exceptions
{
    public class ServiceNameUnknown : ArgumentOutOfRangeException
    {
        public readonly String ServiceName;

        internal ServiceNameUnknown(String name, ServiceIdUnknownException inner) : base($"Unable to locate Service Name: \"{name}\"", inner)
        {
            this.ServiceName = name;
        }
    }
}
