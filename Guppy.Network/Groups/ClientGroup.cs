using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.Logging;

namespace Guppy.Network.Groups
{
    public class ClientGroup : Group
    {
        private ClientPeer _client;

        public ClientGroup(Guid id, ClientPeer client, ILogger log) : base(id, client, log)
        {
            _client = client;

            this.internalMessageHandler.Add("setup:start", this.HandleSetupStartMessage);
            this.internalMessageHandler.Add("user:joined", this.HandleUserJoinedMessage);
            this.internalMessageHandler.Add("user:left", this.HandleUserLeftMessage);
            this.internalMessageHandler.Add("setup:end", this.HandleSetupEndMessage);
        }

        public override void SendMesssage(NetOutgoingMessage om, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, int sequenceChannel = 0)
        {
            _client.SendMessage(om, method, sequenceChannel);
        }

        #region Internal Message Handlers
        private void HandleSetupStartMessage(NetIncomingMessage obj)
        {
            this.updateMessages = this.update_ignoreMessages;
        }

        private void HandleUserJoinedMessage(NetIncomingMessage obj)
        {
            // Select the newly joined user...
            var user = _client.Users.UpdateOrCreate(obj.ReadGuid(), obj);
            // Add the new user to the groups user collection
            this.Users.Add(user);
        }

        private void HandleUserLeftMessage(NetIncomingMessage obj)
        {
            // Remove the user from the internal user collection...
            this.Users.Remove(
                item: this.Users.GetById(obj.ReadGuid()));
        }

        private void HandleSetupEndMessage(NetIncomingMessage obj)
        {
            this.updateMessages = this.update_readMessages;
        }
        #endregion
    }
}
