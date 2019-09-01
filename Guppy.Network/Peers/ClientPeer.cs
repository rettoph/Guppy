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

        #region Public Attributes
        public User User { get; internal set; }
        #endregion

        #region Constructor
        public ClientPeer(NetClient client) : base(client)
        {
            _client = client;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize()
        {
            base.Initialize();

            this.Messages.TryAdd(NetIncomingMessageType.StatusChanged, this.HandleStatusChanged);
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
                user.Dispose();
            }
        }
        #endregion

        #region Message Handlers
        private void HandleStatusChanged(object sender, NetIncomingMessage arg)
        {
            switch(_client.ConnectionStatus)
            {
                case NetConnectionStatus.Connected:
                    _client.ServerConnection.RemoteHailMessage.Position = 0;
                    this.User = this.Users.Create(u => u.Read(_client.ServerConnection.RemoteHailMessage));
                    break;
                case NetConnectionStatus.Disconnected:
                    this.Users.Dispose();
                    this.Groups.Dispose();
                    break;
            }
        }
        #endregion
    }
}
