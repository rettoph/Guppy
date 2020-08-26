using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Exceptions
{
    public class ServiceIdUnknownException : ArgumentOutOfRangeException
    {
        public UInt32 ServiceId;

        internal ServiceIdUnknownException(UInt32 id) : base($"Unable to locate Service Id: \"{id}\"")
        {
            this.ServiceId = id;
        }
    }
}
