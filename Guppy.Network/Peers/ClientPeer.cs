using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using Microsoft.Extensions.Logging;

namespace Guppy.Network.Peers
{
    public class ClientPeer : Peer
    {
        #region Private Fields
        private NetClient _client;
        #endregion

        #region Constructor
        public ClientPeer(NetClient client) : base(client)
        {
            _client = client;
        }
        #endregion

        #region Methods
        public void TryConnect(String host, Int32 port, NetOutgoingMessage hail = null)
        {
            if (_client.ConnectionStatus != NetConnectionStatus.Disconnected)
            {
                this.logger.LogWarning($"Unable to connect. CUrrent connection status is {_client.ConnectionStatus}.");
            }
            else
            {
                this.logger.LogTrace($"Attempting to connect to {host}:{port}...");
                _client.Connect(host, port, hail);
            }
        }
        #endregion
    }
}
