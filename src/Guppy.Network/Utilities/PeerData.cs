using Guppy.Network.Peers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// Simple container meant to hold "global" peer data.
    /// 
    /// If important peer info must be updated, it should be
    /// done here.
    /// </summary>
    public sealed class PeerData
    {
        public Peer Peer { get; set; }
        public NetPeer NetPeer { get; set; }

        internal PeerData()
        {

        }
    }
}
