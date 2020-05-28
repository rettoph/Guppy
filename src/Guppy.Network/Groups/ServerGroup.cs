using Guppy.DependencyInjection;
using Guppy.Network.Enums;
using Guppy.Network.Structs;
using Guppy.Network.Utilities;
using Guppy.Network.Extensions.Lidgren;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Guppy.Network.Groups
{
    public class ServerGroup : Group
    {
        #region Private Fields
        private NetServer _server;
        private IList<NetConnection> _connections;
        private UserNetConnectionDictionary _userConnections;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _connections = new List<NetConnection>();

            provider.Service(out _userConnections);
            provider.Service(out _server);

            this.Users.OnAdded += this.HandleUserJoined;
            this.Users.OnRemoved += this.HandleUserLeft;
        }
        #endregion

        #region Messageable Implementation
        protected override void Send(NetOutgoingMessageConfiguration message)
        {
            if(message.Recipient != default(NetConnection))
                _server.SendMessage(
                    msg: message.Message,
                    recipient: message.Recipient,
                    method: message.Method,
                    sequenceChannel: message.SequenceChannel);
            else if(_connections.Any())
                _server.SendMessage(
                    msg: message.Message, 
                    recipients: _connections, 
                    method: message.Method, 
                    sequenceChannel: message.SequenceChannel);
        }
        #endregion

        #region Event Handlers
        private void HandleUserJoined(IEnumerable<User> sender, User user)
        {
            // Auto add the current group to the user's groups.
            user.Groups.TryAdd(this);

            // Save the new user connection, so they will recieve group messages.
            _connections.Add(_userConnections.Connections[user]);

            // Notify all connected users about the new in group user...
            this.Messages.Create(NetDeliveryMethod.ReliableOrdered, 0).Write("user:joined", om =>
            {
                om.Write(user.Id);
                user.TryWrite(om);
            });
        }

        private void HandleUserLeft(IEnumerable<User> sender, User user)
        {
            // Auto remove the current group from the user's groups.
            user.Groups.TryRemove(this);

            // Remove the user connection, so they will no longer recieve messages.
            _connections.Remove(_userConnections.Connections[user]);

            // Alert all remaining users if any about the user leaving
            this.Messages.Create(NetDeliveryMethod.ReliableOrdered, 0).Write("user:left", om =>
            {
                om.Write(user.Id);
            });            
        }
        #endregion
    }
}
