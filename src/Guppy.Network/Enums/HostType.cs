using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Enums
{
    public enum HostType
    {
        /// <summary>
        /// Indicates that no peer exists on the local machine.
        /// </summary>
        Local,

        /// <summary>
        /// Indicates that a peer has been created on the local machine.
        /// </summary>
        Remote
    }
}
