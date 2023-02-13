using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Enums
{
    [Flags]
    public enum PeerType
    {
        None = 1,
        Client = 2,
        Server = 4,
    }
}
