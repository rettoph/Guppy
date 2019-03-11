using Guppy.Network.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Security.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Guppy.Network.Extensions;
using Guppy.Network.Security.Enums;
using Guppy.Collections;

namespace Guppy.Network.Peers
{
    public class ServerPeer : Peer
    {
        #region Protected Attributes
        protected NetServer server;
        #endregion

        #region Public Attributes
        public IAuthenticationService AuthenticationService { get; private set; }
        #endregion

        #region Constructors
        public ServerPeer(NetPeerConfiguration config, ILogger logger) : base(config, logger)
        {
            this.server = new NetServer(this.config);
            this.peer = this.server;

            this.AuthenticationService = new DefaultAuthenticationService();
        }
        #endregion

        #region Send Message Methods
        public void SendToAll(NetOutgoingMessage om, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced)
        {
            this.server.SendToAll(om, method);
        }
        #endregion

        #region MessageType Handlers
        protected override void ConnectionApproval(NetIncomingMessage im)
        {
            base.ConnectionApproval(im);

            this.logger.LogInformation("New incoming connection request... Attempting authentication.");

            // Create a new user object based on the remote hail message
            var user = new User(im.SenderConnection.RemoteHailMessage.ReadGuid());
            user.Read(im.SenderConnection.RemoteHailMessage);
            user.Set("connection", ClaimType.Protected, im.SenderConnection.RemoteUniqueIdentifier.ToString());

            // Run the requested user through the authenticator...
            var auth = this.AuthenticationService.Authenticate(user);

            // If it was a success, approve the new connection
            // otherwise deny it
            switch (auth.Type)
            {
                case AuthenticateResultType.Success:
                    // Send a success hail to the new user
                    var response = this.CreateMessage();
                    response.Write(auth.Message);
                    user.Write(response, ClaimType.Protected);
                    im.SenderConnection.Approve(response);

                    // Add the user to the user collection
                    this.Users.Add(user);
                    break;
                case AuthenticateResultType.Failure:
                    im.SenderConnection.Deny(auth.Message);
                    break;
            }
        }
    }
    #endregion
}
