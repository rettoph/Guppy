using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Enums
{
    public enum ClaimType
    {
        Private, // Not sent to any peers...
        Protected, // Only sent to the connected peer...
        Public // Sent to all peers...
    }
}
