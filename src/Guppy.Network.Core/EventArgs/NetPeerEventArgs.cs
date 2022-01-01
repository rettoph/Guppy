using Guppy.Threading.Interfaces;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.EventArgs
{
    public sealed class NetPeerEventArgs : System.EventArgs, IData
    {
        public readonly NetPeer NetPeer;

        public NetPeerEventArgs(NetPeer netPeer)
        {
            this.NetPeer = netPeer;
        }
    }
}
