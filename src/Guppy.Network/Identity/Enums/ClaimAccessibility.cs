using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Identity.Enums
{
    public enum ClaimAccessibility
    {
        Public, // Sent to all peers...
        Protected, // Only sent to the connected peer...
        Private, // Not sent to any peers...
    }
}
