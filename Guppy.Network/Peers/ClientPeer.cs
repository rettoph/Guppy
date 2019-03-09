using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    public class ClientPeer : Peer
    {
        #region protected Attributes
        protected NetClient client;
        #endregion

        public ClientPeer(NetPeerConfiguration config, ILogger logger) : base(config, logger)
        {
            this.client = new NetClient(config);
            this.peer = this.client;
        }

        #region Connect Methods
        public void Connect(String host, Int32 port, User user)
        {
            var hail = this.CreateMessage();
            user.Write(hail);

            this.peer.Connect(host, port, hail);
        }
        #endregion
    }
}
