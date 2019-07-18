using Guppy.Network.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Security.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Security.Enums;
using Guppy.Collections;
using System.Linq;
using Guppy.Network.Groups;
using Guppy.Network.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Enums;

namespace Guppy.Network.Peers
{
    public class ServerPeer : Peer
    {
        #region Private Fields
        private Dictionary<NetConnection, User> _approvedUsers;
        #endregion

        #region Protected Attributes
        protected NetServer server;
        #endregion

        #region Public Attributes
        public IAuthenticationService AuthenticationService { get; set; }
        #endregion

        #region Events
        public event EventHandler<User> OnUserConnected;
        public event EventHandler<User> OnUserDisconnected;
        #endregion

        #region Constructors
        public ServerPeer(NetPeerConfiguration config, NetOutgoingMessageConfigurationPool netOutgoingMessageConfigurationPool, GlobalUserCollection users, GroupCollection groups, IServiceProvider provider) : base(config, netOutgoingMessageConfigurationPool, users, groups, provider)
        {
            _approvedUsers = new Dictionary<NetConnection, User>();

            this.server = new NetServer(this.config);
            this.peer = this.server;

            this.AuthenticationService = new DefaultAuthenticationService();
        }
        #endregion

        #region Methods
        public NetConnection GetNetConnectionById(Int64 id)
        {
            return this.server.Connections.FirstOrDefault(c => c.RemoteUniqueIdentifier == id);
        }
        public NetConnection GetNetConnectionByUser(User user)
        {
            return this.GetNetConnectionById(user.NetId);
        }

        public NetServer GetNetServer()
        {
            return this.server;
        }
        #endregion

        #region MessageType Handlers
        protected override void HandleConnectionApproval(NetIncomingMessage im)
        {
            base.HandleConnectionApproval(im);

            this.logger.LogInformation("New incoming connection request... Attempting authentication.");

            // Create a new user object based on the remote hail message
            im.SenderConnection.RemoteHailMessage.ReadGuid();
            var user = ActivatorUtilities.CreateInstance<User>(this.provider, Guid.NewGuid(), im.SenderConnection.RemoteUniqueIdentifier);
            user.Read(im.SenderConnection.RemoteHailMessage);

            // Run the requested user through the authenticator...
            var auth = this.AuthenticationService.Authenticate(user);

            // If it was a success, approve the new connection
            // otherwise deny it
            switch (auth.Type)
            {
                case AuthenticateResultType.Success:
                    // Send a success hail to the new user
                    var response = this.peer.CreateMessage();
                    response.Write(auth.Message);
                    user.Write(response, ClaimType.Protected);
                    im.SenderConnection.Approve(response);
                    // Add the user to the temp approved users table
                    _approvedUsers.Add(im.SenderConnection, user);
                    break;
                case AuthenticateResultType.Failure:
                    im.SenderConnection.Deny(auth.Message);
                    break;
            }
        }

        protected override void HandleStatusChanged(NetIncomingMessage im)
        {
            base.HandleStatusChanged(im);

            switch (im.SenderConnection.Status)
            {
                case NetConnectionStatus.Connected:
                    // Load the user linked to the current connection
                    var newUser = _approvedUsers[im.SenderConnection];
                    // Add the user to the users collection
                    this.Users.Add(newUser);
                    // Remove the user from the approved table
                    _approvedUsers.Remove(im.SenderConnection);
                    this.OnUserConnected?.Invoke(this, newUser);
                    break;
                case NetConnectionStatus.Disconnected:
                    var oldUser = this.Users.GetByNetConnection(im.SenderConnection);
                    this.Users.Remove(oldUser);
                    this.OnUserDisconnected?.Invoke(this, oldUser);
                    break;
            }
        }

        #endregion

        #region IMessageTarget Implementation
        public override void Flush()
        {
            NetOutgoingMessageConfiguration config;
            if (this.Users.Count() > 0)
            {
                while (this.queuedMessages.Count > 0)
                {
                    config = this.queuedMessages.Dequeue();

                    if (config.Target == null)
                        this.server.SendToAll(config.Message, null, config.Method, config.SequenceChannel);
                    else
                        this.server.SendMessage(config.Message, config.Target, config.Method, config.SequenceChannel);

                    this.netOutgoingMessageConfigurationPool.Put(config);
                }
            }
            else
            {
                while (this.queuedMessages.Count > 0)
                    this.netOutgoingMessageConfigurationPool.Put(this.queuedMessages.Dequeue());
            }
        }
        #endregion
    }
}
