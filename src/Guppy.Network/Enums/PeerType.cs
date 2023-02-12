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
        Client = 1,
        Server = 2
    }
}
