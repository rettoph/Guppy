using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Exceptions
{
    public class ServiceTypeUnknown : ArgumentOutOfRangeException
    {
        public readonly Type ServiceType;

        internal ServiceTypeUnknown(Type type, ServiceIdUnknownException inner) : base($"Unable to locate Service Type: \"{type}\"", inner)
        {
            this.ServiceType = type;
        }
    }
}
