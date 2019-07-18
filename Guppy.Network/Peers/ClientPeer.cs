using Guppy.Network.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    public class ClientPeer : Peer
    {
        #region Protected Attributes
        protected NetClient client;
        #endregion

        #region Public Attributes
        public User CurrentUser { get; private set; }
        #endregion

        #region Constructors
        public ClientPeer(NetPeerConfiguration config, NetOutgoingMessageConfigurationPool netOutgoingMessageConfigurationPool, GlobalUserCollection users, GroupCollection groups, IServiceProvider provider) : base(config, netOutgoingMessageConfigurationPool, users, groups, provider)
        {
            this.client = new NetClient(config);
            this.peer = this.client;
        }
        #endregion

        #region Connect Methods
        public void Connect(String host, Int32 port, User user)
        {
            var hail = this.peer.CreateMessage();
            user.Write(hail, ClaimType.Protected);

            this.peer.Connect(host, port, hail);
        }
        #endregion

        #region Methods
        public NetClient GetNetClient()
        {
            return this.client;
        }
        #endregion

        #region MessageType Handlers
        protected override void HandleStatusChanged(NetIncomingMessage im)
        {
            switch (im.SenderConnection.Status)
            {
                case NetConnectionStatus.None:
                    break;
                case NetConnectionStatus.InitiatedConnect:
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    break;
                case NetConnectionStatus.RespondedConnect:
                    break;
                case NetConnectionStatus.Connected:
                    // When the client is connected, we want to create a new user object based on data passed
                    // from the server
                    var message = im.SenderConnection.RemoteHailMessage.ReadString();
                    var user = ActivatorUtilities.CreateInstance<User>(this.provider, im.SenderConnection.RemoteHailMessage.ReadGuid(), default(Int64));
                    user.Read(im.SenderConnection.RemoteHailMessage);
                    // Add the user to the user collection
                    this.Users.Add(user);
                    // Set the new user as the current user
                    this.CurrentUser = user;
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    break;
            }

            // Call base method after, since a post event is called.
            base.HandleStatusChanged(im);
        }
        #endregion

        #region IMessageTarget Implementation
        public override void Flush()
        {
            NetOutgoingMessageConfiguration config;

            while(this.queuedMessages.Count > 0)
            {
                config = this.queuedMessages.Dequeue();

                this.client.SendMessage(config.Message, config.Method, config.SequenceChannel);

                this.netOutgoingMessageConfigurationPool.Put(config);
            }
        }
        #endregion
    }
}
