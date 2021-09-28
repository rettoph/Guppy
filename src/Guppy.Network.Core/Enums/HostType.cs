using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Enums
{
    public enum HostType
    {
        /// <summary>
        /// Indicates that no peer exists on the local machine.
        /// </summary>
        Local,

        /// <summary>
        /// Indicates that a peer has been created on the locla machine.
        /// </summary>
        Remote
    }
}
