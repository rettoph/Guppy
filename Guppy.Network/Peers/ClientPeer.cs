using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Security;
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
        public void TryConnect(String host, Int32 port, User user)
        {
            if (_client.ConnectionStatus != NetConnectionStatus.Disconnected)
            {
                this.logger.LogWarning($"Unable to connect. Current connection status is {_client.ConnectionStatus}.");
            }
            else
            {
                this.logger.LogTrace($"Attempting to connect to {host}:{port}...");
                var hail = _client.CreateMessage();
                user.Write(hail);
                _client.Connect(host, port, hail);
            }
        }
        #endregion
    }
}
