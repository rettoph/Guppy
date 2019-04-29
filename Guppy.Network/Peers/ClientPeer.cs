using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
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

        #region Methods
        protected internal override Group CreateGroup(Guid id)
        {
            return new ClientGroup(id, this, this.logger);
        }
        public NetClient GetNetClient()
        {
            return this.client;
        }
        #endregion

        #region MessageType Handlers
        protected override void HandleStatusChanged(NetIncomingMessage im)
        {
            base.HandleStatusChanged(im);

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

        #region Send Message Methods
        public void SendMessage(NetOutgoingMessage om, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, Int32 sequenceChannel = 0)
        {
            this.client.SendMessage(om, method, sequenceChannel);
        }
        #endregion
    }
}
