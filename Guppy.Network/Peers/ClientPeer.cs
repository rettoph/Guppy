using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Configurations;
using Guppy.Network.Security.Authentication;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using Microsoft.Extensions.Logging;

namespace Guppy.Network.Peers
{
    /// <summary>
    /// Peer designed specifically for client side
    /// communications.
    /// </summary>
    public class ClientPeer : Peer
    {
        #region Private Fields
        private NetClient _client;
        #endregion

        #region Constructor
        public ClientPeer(NetClient client, Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool) : base(client, outgoingMessageConfigurationPool)
        {
            _client = client;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize()
        {
            base.Initialize();

            // Add event handlers...
            this.Events.AddDelegate<NetIncomingMessage>("recieved:status-changed", this.HandleStatusChanged);
        }
        #endregion

        #region Helper Methods
        public void TryConnect(String host, Int32 port, User identity)
        {
            if (_client.ConnectionStatus != NetConnectionStatus.Disconnected)
                throw new Exception("Unable to connect! Client has already started a connection.");

            this.logger.LogInformation($"Attempting to connect to {host}:{port}...");

            var hail = _client.CreateMessage();
            identity.TryWrite(hail);
            _client.Connect(host, port, hail);
        }
        #endregion

        #region Target Implementation
        public override void SendMessage(NetOutgoingMessageConfiguration om)
        {
            _client.SendMessage(om.Message, _client.ServerConnection, om.Method, om.SequenceChannel);
        }
        #endregion

        #region Event Handlers
        private void HandleStatusChanged(object sender, NetIncomingMessage arg)
        {
            this.logger.LogDebug($"Client connection status updated => {(NetConnectionStatus)arg.ReadByte()}");
        }
        #endregion
    }
}
