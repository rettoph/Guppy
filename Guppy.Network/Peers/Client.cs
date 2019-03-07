using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    public class Client : Peer
    {
        #region protected Attributes
        protected NetClient client;
        #endregion

        public Client(NetPeerConfiguration config, ILogger logger) : base(config, logger)
        {
            this.client = new NetClient(config);
            this.peer = this.client;
        }
    }
}
