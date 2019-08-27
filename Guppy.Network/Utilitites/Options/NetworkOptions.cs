using Guppy.Network.Peers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilitites.Options
{
    internal sealed class NetworkOptions
    {
        public NetPeer NetPeer { get; internal set; }
        public Peer Peer { get; internal set; }
    }
}
