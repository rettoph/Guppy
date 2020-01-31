using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Configurations;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
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
        public NetConnection ServerConnection { get => _client.ServerConnection; }
        public NetConnectionStatus ConnectionStatus { get => _client.ConnectionStatus; }
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

            this.MessagesTypes.TryAdd(NetIncomingMessageType.StatusChanged, this.HandleStatusChanged);
        }
        #endregion

        #region Peer Implementation
        /// <inheritdoc />
        protected internal override Type GroupType()
        {
            return typeof(ClientGroup);
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
                user.TryWrite(hail);
                _client.Connect(host, port, hail);
                this.User = user;
            }
        }
        #endregion

        #region Message Handlers
        private void HandleStatusChanged(object sender, NetIncomingMessage arg)
        {
            switch(_client.ConnectionStatus)
            {
                case NetConnectionStatus.Connected:
                    this.User.Dispose();
                    this.User = this.Users.Create(u =>
                    {
                        u.SetId(_client.ServerConnection.RemoteHailMessage.ReadGuid());
                        u.TryRead(_client.ServerConnection.RemoteHailMessage);
                    });
                    break;
                case NetConnectionStatus.Disconnecting:
                    this.Users.Dispose();
                    this.Groups.Dispose();
                    break;
            }
        }
        #endregion
    }
}
