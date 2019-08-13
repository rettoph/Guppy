using Guppy.Network.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Security.Authentication;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Groups
{
    public class ServerGroup : Group
    {
        #region Private Fields
        private NetServer _server;
        private List<NetConnection> _connections;
        #endregion

        #region Constructor
        public ServerGroup(UserCollection users, NetServer server, Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool) : base(users, server, outgoingMessageConfigurationPool)
        {
            _server = server;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _connections = new List<NetConnection>();

            this.Users.Events.AddDelegate<User>("added", this.HandleUserAdded);
            this.Users.Events.AddDelegate<User>("removed", this.HandleUserRemoved);
        }
        #endregion

        #region Target Implmentation
        public override void SendMessage(NetOutgoingMessageConfiguration om)
        {
            if (om.Recipient == null)
                _server.SendMessage(om.Message, _connections, om.Method, om.SequenceChannel);
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(object sender, User user)
        {
            _connections.Remove(user.NetConnection);

            // Boradcast the new user to all users connected to the group
            var added = this.CreateMessage(type: "added:user");
            user.TryWrite(added);
        }

        private void HandleUserRemoved(object sender, User user)
        {
            _connections.Add(user.NetConnection);
        }
        #endregion
    }
}
