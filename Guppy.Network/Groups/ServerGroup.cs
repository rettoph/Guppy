using Guppy.Network.Collections;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Microsoft.Extensions.Logging;
using Guppy.Network.Configurations;

namespace Guppy.Network.Groups
{
    public class ServerGroup : Group
    {
        private ServerPeer _server;
        private NetServer _netServer;
        private List<NetConnection> _connections;

        public IReadOnlyCollection<NetConnection> Connections { get { return _connections; } }

        #region Events
        /// <summary>
        /// Event called when the setup function is running for a specific user.
        /// </summary>
        public event EventHandler<User> OnSetup;
        #endregion

        #region Constructors
        public ServerGroup(Guid id, ServerPeer server, NetServer netServer, NetOutgoingMessageConfigurationPool netOutgoingMessageConfigurationPool, IServiceProvider provider) : base(id, server, netServer, netOutgoingMessageConfigurationPool, provider)
        {
            _server = server;
            _netServer = netServer;
            _connections = new List<NetConnection>();

            this.Users.Added += this.HandleUserAdded;
            this.Users.Removed += this.HandleUserRemoved;
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(object sender, User user)
        {
            // Load the new users connection...
            var newConnection = _server.GetNetConnectionById(user.NetId);

            if (newConnection != null)
            {
                NetOutgoingMessage userUpdate;
                // Broadcast a message of the new users arrival to all users (this will include the new user at flush time)
                userUpdate = this.CreateMessage("user:joined", NetDeliveryMethod.ReliableOrdered, 0);
                user.Write(userUpdate);

                
                foreach (User knownUser in this.Users)
                { // Send an update to the new user alerting them of all pre-existing users in the group
                    if (knownUser != user)
                    { // Ensure that the new user is not re-sent themselves.
                        userUpdate = this.CreateMessage("user:joined", NetDeliveryMethod.ReliableOrdered, 0, newConnection);
                        knownUser.Write(userUpdate);
                    }
                }

                this.OnSetup?.Invoke(this, user);

                // Add the new user connection to the connections list
                _connections.Add(newConnection);
            }   
        }

        private void HandleUserRemoved(object sender, User user)
        {
            // Remove the user's connection from the NetConnection list
            _connections.Remove(_server.GetNetConnectionByUser(user));

            if (_connections.Count > 0)
            {
                // Send an update message to all remaining users
                var userUpdate = this.CreateMessage("user:left", NetDeliveryMethod.ReliableOrdered, 0);
                userUpdate.Write(user.Id);
            }
        }
        #endregion

        #region IMessageTarget Implementation
        public override void Flush()
        {
            NetOutgoingMessageConfiguration config;

            if (_connections.Count > 0)
            {
                while (this.queuedMessages.Count > 0)
                {
                    config = this.queuedMessages.Dequeue();

                    if (config.Target == null)
                        _netServer.SendMessage(config.Message, _connections, config.Method, config.SequenceChannel);
                    else
                        _netServer.SendMessage(config.Message, config.Target, config.Method, config.SequenceChannel);

                    this.netOutgoingMessageConfigurationPool.Put(config);
                }
            }
            else
            {
                while (this.queuedMessages.Count > 0)
                    this.netOutgoingMessageConfigurationPool.Put(this.queuedMessages.Dequeue());
            }
        }
        #endregion
    }
}
