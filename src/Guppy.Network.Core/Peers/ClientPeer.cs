using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network.Channels;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Extensions.Security;
using Guppy.Network.Interfaces;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public sealed class ClientPeer : Peer
    {
        #region Static Fields
        private static ServiceConfigurationKey ChannelServiceConfigurationId = ServiceConfigurationKey.From<ClientChannel>();
        #endregion

        #region Private Fields
        private NetClient _client;
        #endregion

        #region Peer Implementation
        protected override ServiceConfigurationKey channelServiceConfigurationkey => ClientPeer.ChannelServiceConfigurationId;
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.StatusChanged] += this.HandleIncomingStatusChangedMessageTypeRecieved;
        }

        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _client);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _client = null;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.StatusChanged] -= this.HandleIncomingStatusChangedMessageTypeRecieved;
        }
        #endregion

        #region Methods
        public override async Task StopAsync()
        {
            await base.StopAsync();

            _client.Disconnect("Goodbye.");
        }

        public void TryConnect(String host, Int32 port, params Claim[] claims)
        {
            var user = this.CreateUser(claims);
            var hail = _client.CreateMessage();
            user.TryWrite(hail);

            _client.Connect(
                host: host,
                port: port,
                hailMessage: hail);
        }
        #endregion

        #region Event Handlers
        private void HandleIncomingStatusChangedMessageTypeRecieved(IPeer sender, NetIncomingMessage im)
        {
            switch(im.SenderConnection.Status)
            {
                case NetConnectionStatus.Connected:
                    // The localhail contains the current user first...
                    this.CurrentUser = this.Users.GetOrCreate(im.SenderConnection.RemoteHailMessage.ReadGuid());
                    this.CurrentUser.TryRead(im.SenderConnection.RemoteHailMessage);
                    this.CurrentUser.Connection = im.SenderConnection;
                    break;
            }
        }
        #endregion
    }
}
