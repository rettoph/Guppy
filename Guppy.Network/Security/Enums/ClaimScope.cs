using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Enums
{
    /// <summary>
    /// Scope representing what security level
    /// a value is.
    /// </summary>
    public enum ClaimScope
    {
        // Shared between all peers
        Public,

        // Shared specifically between the server and client the claim belongs to
        Protected,

        // Saved on a single peer, not shared at all
        Private
    }
}
