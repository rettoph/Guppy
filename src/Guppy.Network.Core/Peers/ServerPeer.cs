using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists.Interfaces;
using Guppy.Network.Channels;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Extensions.Security;
using Guppy.Network.Interfaces;
using Guppy.Network.Security;
using Guppy.Network.Services;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Peers
{
    public class ServerPeer : Peer
    {
        #region Static Fields
        private static ServiceConfigurationKey ChannelServiceConfigurationId = ServiceConfigurationKey.From<ServerChannel>();
        #endregion

        #region Private Fields
        private NetServer _server;
        private HashSet<IUser> _approvedUsers;
        #endregion

        #region Peer Implementation
        protected override ServiceConfigurationKey channelServiceConfigurationkey => ServerPeer.ChannelServiceConfigurationId;
        #endregion

        #region Events
        public event ValidateEventDelegate<NetIncomingMessage, IUser> ValidateConnectionRequest;
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            _approvedUsers = new HashSet<IUser>();

            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.ConnectionApproval] += this.HandleIncomingConnectionApprovalMessageTypeRecieved;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.StatusChanged] += this.HandleIncomingStatusChangedMessageTypeRecieved;
        }

        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _server);
        }

        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            this.CurrentUser = this.Users.GetOrCreate(Guid.Empty);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _approvedUsers.Clear();

            _server = null;
        }

        protected override void Dispose()
        {
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.ConnectionApproval] -= this.HandleIncomingConnectionApprovalMessageTypeRecieved;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.StatusChanged] -= this.HandleIncomingStatusChangedMessageTypeRecieved;

            base.Dispose();
        }
        #endregion

        #region Incoming Message Handlers
        private void HandleIncomingConnectionApprovalMessageTypeRecieved(IPeer sender, NetIncomingMessage im)
        {
            // Skip the recieved user id, as the server will create a new one.
            im.SenderConnection.RemoteHailMessage.ReadGuid();

            // Create a new user & read the incoming data..
            var user = this.CreateUser();
            user.TryRead(im.SenderConnection.RemoteHailMessage);
            user.Connection = im.SenderConnection;

            // Attempt to validate the incoming connection request.
            if(this.ValidateConnectionRequest.Validate(im, user, true))
            {
                var hail = _server.CreateMessage();
                user.TryWrite(hail);
                im.SenderConnection.Approve(hail);

                _approvedUsers.Add(user);
            }
            else
            {
                im.SenderConnection.Deny();
            }
        }

        private void HandleIncomingStatusChangedMessageTypeRecieved(IPeer sender, NetIncomingMessage im)
        {
            IUser user;

            switch(im.SenderConnection.Status)
            {
                case NetConnectionStatus.Connected:
                    user = _approvedUsers.First(user => user.Connection == im.SenderConnection);
                    _approvedUsers.Remove(user);
                    this.Users.TryAdd(user);
                    break;
                case NetConnectionStatus.Disconnected:
                    user = this.Users.First(user => user.Connection == im.SenderConnection);
                    user.TryRelease();
                    break;
            }
        }
        #endregion
    }
}