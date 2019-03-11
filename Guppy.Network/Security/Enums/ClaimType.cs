using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Enums
{
    public enum ClaimType : Byte
    {
        /// <summary>
        /// All peers will know this user claim
        /// </summary>
        Public = 0,
        /// <summary>
        /// Onle the client and server will know the users claim
        /// </summary>
        Protected = 1,
        /// <summary>
        /// Only the peer with the current client will know this claim
        /// </summary>
        Private = 2,
    }
}
