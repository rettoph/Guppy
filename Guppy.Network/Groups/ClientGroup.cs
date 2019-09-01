using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.Logging;

namespace Guppy.Network.Groups
{
    public sealed class ClientGroup : Group
    {
        #region Private Fields
        private ClientPeer _client;
        #endregion

        #region Constructor
        public ClientGroup(ClientPeer client, CreatableCollection<User> users, Peer peer) : base(users, peer)
        {
            _client = client;
        }
        #endregion

        #region Lifecycle Methods 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Messages.TryAdd("user:joined", this.HandleUserJoinedMessage);
            this.Messages.TryAdd("user:left", this.HandleUserLeftMessage);
        }
        #endregion

        #region Message Handlers
        /// <summary>
        /// Automatically add a user instance to the current groups user collection
        /// based on data recieved from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void HandleUserJoinedMessage(object sender, NetIncomingMessage arg)
        {
            // Load the user from the cached users collection, if it exists...
            var user = _client.Users.GetOrCreateById(arg.ReadGuid());
            // Update the user data...
            user.Read(arg);
            // Add the user to the groups user collection
            this.Users.Add(user);
        }

        /// <summary>
        /// Automatically remove a user from the local user collection
        /// based on data recieved from the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void HandleUserLeftMessage(object sender, NetIncomingMessage arg)
        {
            this.Users.Remove(this.Users.GetById(arg.ReadGuid()));
        }
        #endregion
    }
}
