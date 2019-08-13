using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
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
        private User _identity;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The current client's identity.
        /// 
        /// If the client is not connected to a server this value
        /// will be null.
        /// </summary>
        public User Identity {
            get {
                return (_client.ConnectionStatus == NetConnectionStatus.Connected ? _identity : null);
            }
        }
        #endregion

        #region Constructor
        public ClientPeer(NetClient client, EntityCollection entities, Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool) : base(client, entities, outgoingMessageConfigurationPool)
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
            identity.Dispose();
            _client.Connect(host, port, hail);
        }

        protected override Type GroupType()
        {
            return typeof(ClientGroup);
        }
        #endregion

        #region Target Implementation
        public override void SendMessage(NetOutgoingMessageConfiguration om)
        {
            _client.SendMessage(om.Message, _client.ServerConnection, om.Method, om.SequenceChannel);
        }
        #endregion

        #region Event Handlers
        private void HandleStatusChanged(object sender, NetIncomingMessage im)
        {
            im.Position = 0;

            // The new status value...
            var status = (NetConnectionStatus)im.ReadByte();
            this.logger.LogDebug($"Client connection status updated => {status}");

            switch(status)
            {
                case NetConnectionStatus.Connected:
                    // Read the incoming user data...
                    this.TryCreateUser(u =>
                    {
                        u.SetId(im.SenderConnection.RemoteHailMessage.ReadGuid());
                        u.TryRead(im.SenderConnection.RemoteHailMessage);
                    });
                    break;
            }
        }
        #endregion
    }
}
