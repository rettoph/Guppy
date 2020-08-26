using Guppy.DependencyInjection;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Peers;
using Guppy.Network.Structs;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network.Groups
{
    public class ClientGroup : Group
    {
        #region Private Fields
        private NetClient _client;
        private ClientPeer _clientPeer;
        private ServiceProvider _provider;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            provider.Service(out _client);
            provider.Service(out _clientPeer);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Messages.Set("user:joined", this.HandleUserJoinedMessage);
            this.Messages.Set("user:left", this.HandleUserLeftMessage);
        }
        #endregion

        #region Messageable Implementation
        protected override void Send(NetOutgoingMessageConfiguration message)
        {
            _client.SendMessage(
                msg: message.Message,
                method: message.Method,
                sequenceChannel: message.SequenceChannel);
        }
        #endregion

        #region Message Handlers
        private void HandleUserJoinedMessage(NetIncomingMessage im)
        {
            var id = im.ReadGuid();
            var user = _clientPeer.Users.GetById(id);

            if (user == default(User))
            { // Create a new user if one doesnt exist yet...
                user = _provider.GetService<User>((u, p, c) =>
                {
                    u.Id = id;
                    u.TryRead(im);
                });
                _clientPeer.Users.TryAdd(user);
            }
            else // Update the pre-existing user
                user.TryRead(im);

            // Add the user into the internal client group...
            this.Users.TryAdd(user);
            user.Groups.TryAdd(this);
        }

        private void HandleUserLeftMessage(NetIncomingMessage im)
        {
            var user = this.Users.GetById(im.ReadGuid());
            this.Users.TryRemove(user);
            user.Groups.TryRemove(this);

            // Automatically dispose of the recieved user if it is not in any shared groups...
            if (user != _clientPeer.CurrentUser && user.Groups.Count == 0)
                user.TryDispose();
        }
        #endregion
    }
}
