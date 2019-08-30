using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;

namespace Guppy.Network.Peers
{
    public class ServerPeer : Peer
    {
        #region Constructor
        public ServerPeer(NetServer server) : base(server)
        {
        }
        #endregion
    }
}
