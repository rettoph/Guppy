using Guppy.Network.Extensions;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    public class ClientPeer : Peer
    {
        #region protected Attributes
        protected NetClient client;
        #endregion

        public ClientPeer(NetPeerConfiguration config, ILogger logger) : base(config, logger)
        {
            this.client = new NetClient(config);
            this.peer = this.client;
        }

        #region Connect Methods
        public void Connect(String host, Int32 port, User user)
        {
            var hail = this.CreateMessage();
            user.Write(hail, ClaimType.Protected);

            this.peer.Connect(host, port, hail);
        }
        #endregion

        #region MessageType Handlers
        protected override void StatusChanged(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.SenderConnection.Status}");

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
                    var user = new User(im.SenderConnection.RemoteHailMessage.ReadGuid());
                    user.Read(im.SenderConnection.RemoteHailMessage);
                    // Add the user to the user collection
                    this.Users.Add(user);
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    break;
            }
        }
        #endregion
    }
}
