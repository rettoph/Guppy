using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Collections;
using Guppy.Network.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Extensions.DependencyInjection;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using Guppy.Network.Security.Authentication;
using Guppy.Network.Security.Authentication.Authenticators;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Network.Peers
{
    /// <summary>
    /// Peer designed specifically for server side
    /// communications.
    /// </summary>
    public class ServerPeer : Peer
    {
        #region Private Fields
        private NetServer _server;
        private IEnumerable<IAuthenticator> _authenticators;
        private UserCollection _approvedUsers { get; set; }
        #endregion

        #region Public Attributes
        public UserCollection Users { get; private set; }
        #endregion

        #region Constructor
        public ServerPeer(NetServer server, EntityCollection entities, UserCollection users, Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool) : base(server, entities, outgoingMessageConfigurationPool)
        {
            _server = server;

            this.Users = users;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _authenticators = provider.GetAuthenticators();
            _approvedUsers = provider.GetService<UserCollection>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Events.AddDelegate<NetIncomingMessage>("recieved:connection-approval", this.HandleConnectionApprovalMessage);
            this.Events.AddDelegate<NetIncomingMessage>("recieved:status-changed", this.HandleStatusChanged);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Users.Dispose();
        }
        #endregion

        #region Helper Methods
        protected override Type GroupType()
        {
            return typeof(ServerGroup);
        }
        #endregion

        #region Target Implementation
        public override void SendMessage(NetOutgoingMessageConfiguration om)
        {
            if (om.Recipient == null)
                _server.SendToAll(om.Message, null, om.Method, om.SequenceChannel);
            else
                _server.SendMessage(om.Message, om.Recipient, om.Method, om.SequenceChannel);
        }
        #endregion

        #region Message Handlers
        /// <summary>
        /// Handle a new incoming connection approval request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="im"></param>
        private void HandleConnectionApprovalMessage(object sender, NetIncomingMessage im)
        {
            // Reset the incoming message position, incase its already been read from
            im.Position = 0;

            // Load in the user recieved in the clients hail message...
            var user = this.TryCreateUser(u =>
            {
                // Read the recieved data from the client...
                u.TryRead(im);
                // Ensure that the new user recieves a random id...
                u.SetId(Guid.NewGuid());
                // Save the users NetConnection...
                u.NetConnection = im.SenderConnection;
            });

            // Iterate though all authenticators...
            foreach(IAuthenticator authenticator in _authenticators)
                if(!authenticator.Validate(user, im))
                { // If a validator fails...
                    im.SenderConnection.Deny(authenticator.GetResponse(user, im));
                    return;
                }

            // If none of the validators fail.. approve the new connection.
            var hail = _server.CreateMessage();
            hail.Write(user.Id);
            user.TryWrite(hail);
            im.SenderConnection.Approve(hail);

            // Add the user to the approved users list temporarily
            _approvedUsers.Add(user);
        }

        /// <summary>
        /// When a connection's status is changed, we must
        /// update the public users list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="im"></param>
        private void HandleStatusChanged(object sender, NetIncomingMessage im)
        { 
            // Load the net connections new status.
            im.Position = 0;
            var status = (NetConnectionStatus)im.ReadByte();
            User user;

            switch(status)
            {
                case NetConnectionStatus.Connected:
                    // When a connection is established, we can remove the user from the approved users list
                    user = _approvedUsers.GetByNetConnection(im.SenderConnection);
                    _approvedUsers.Remove(user);
                    // And add the user to the connected users list
                    this.Users.Add(user);
                    break;
                case NetConnectionStatus.Disconnected:
                    // Remove the old user from the user list
                    user = this.Users.GetByNetConnection(im.SenderConnection);
                    user.Dispose();
                    break;
            }
        }
        #endregion
    }
}
