using Guppy.Network.Collections;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions;

namespace Guppy.Network.Groups
{
    public class ServerGroup : Group
    {
        private ServerPeer _server;
        private List<NetConnection> _connections;

        public ServerGroup(Guid id, ServerPeer server) : base(id, server)
        {
            _server = server;
            _connections = new List<NetConnection>();

            this.Users.Added += this.HandleUserAdded;
            this.Users.Removed += this.HandleUserRemoved;
        }

        #region Send Message Methods
        public void SendMesssage(NetOutgoingMessage om, User user, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, Int32 sequenceChannel = 0)
        {
            _server.SendMessage(om, _server.GetNetConnectionByUser(user), method, sequenceChannel);
        }
        public override void SendMesssage(NetOutgoingMessage om, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, int sequenceChannel = 0)
        {
            _server.SendMessage(om, _connections, method, sequenceChannel);
        }
        public void SendMesssage(NetOutgoingMessage om, NetConnection recipient, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, Int32 sequenceChannel = 0)
        {
            _server.SendMessage(om, recipient, method, sequenceChannel);
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(object sender, User user)
        {
            // Load the new users connection...
            var newConnection = _server.GetNetConnectionById(Int64.Parse(user.Get("connection")));

            NetOutgoingMessage userUpdate;
            if (_connections.Count > 0)
            { // Ensure that users exist before broadcasting an update...
                // Alert all pre-existing users of the new user
                userUpdate = this.CreateMessage("user:joined", MessageType.Internal);
                user.Write(userUpdate);
                this.SendMesssage(userUpdate, NetDeliveryMethod.ReliableOrdered, 0);
            }

            // Alert the new user that their setup is starting
            var setupStart = this.CreateMessage("setup:start", MessageType.Internal);
            this.SendMesssage(setupStart, newConnection, NetDeliveryMethod.ReliableOrdered, 0);

            foreach (User knownUser in this.Users)
            { // Send an update to the new user alerting them of all pre-existing users in the group
                userUpdate = this.CreateMessage("user:joined", MessageType.Internal);
                knownUser.Write(userUpdate);
                this.SendMesssage(userUpdate, newConnection, NetDeliveryMethod.ReliableOrdered, 0);
            }

            // Alert the new user that their setup is ending
            var setupEnd = this.CreateMessage("setup:end", MessageType.Internal);
            this.SendMesssage(setupEnd, newConnection, NetDeliveryMethod.ReliableOrdered, 0);

            // Add the new user connection to the connections list
            _connections.Add(newConnection);
        }

        private void HandleUserRemoved(object sender, User user)
        {
            // Remove the user's connection from the NetConnection list
            _connections.Remove(_server.GetNetConnectionByUser(user));
            // Send an update message to all remaining users
            var userUpdate = this.CreateMessage("user:left", MessageType.Internal);
            userUpdate.Write(user.Id);
            this.SendMesssage(userUpdate, NetDeliveryMethod.ReliableOrdered, 0);
        }
        #endregion
    }
}
