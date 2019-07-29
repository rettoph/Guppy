using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Configurations;
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
        private NetClient _netClient;

        #region Constructors
        public ClientGroup(Guid id, ClientPeer client, NetClient netClient, NetOutgoingMessageConfigurationPool netOutgoingMessageConfigurationPool, IServiceProvider provider) : base(id, client, netClient, netOutgoingMessageConfigurationPool, provider)
        {
            _client = client;
            _netClient = netClient;
        }
        #endregion

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            this.Messages.AddHandler("user:joined", this.HandleUserJoinedMessage);
            this.Messages.AddHandler("user:left", this.HandleUserLeftMessage);
        }
        #endregion

        #region Internal Message Handlers
        private void HandleUserJoinedMessage(Object sender, NetIncomingMessage obj)
        {
            // Select the newly joined user...
            var user = _client.Users.UpdateOrCreate(obj.ReadGuid(), obj);
            // Add the new user to the groups user collection
            this.Users.Add(user);
        }

        private void HandleUserLeftMessage(Object sender, NetIncomingMessage obj)
        {
            // Remove the user from the internal user collection...
            this.Users.Remove(
                item: this.Users.GetById(obj.ReadGuid()));
        }
        #endregion

        #region IMessageTarget Implementation
        public override void Flush()
        {
            NetOutgoingMessageConfiguration config;

            while (this.queuedMessages.Count > 0)
            {
                config = this.queuedMessages.Dequeue();

                _netClient.SendMessage(config.Message, config.Method, config.SequenceChannel);

                this.netOutgoingMessageConfigurationPool.Put(config);
            }
        }
        #endregion
    }
}
