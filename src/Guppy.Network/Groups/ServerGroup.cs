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
using System.Collections.Concurrent;
using Microsoft.Xna.Framework;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network.Groups
{
    public class ServerGroup : Group
    {
        #region Private Fields
        private NetServer _server;
        private IList<NetConnection> _connections;
        private UserNetConnectionDictionary _userConnections;

        private User _user;
        private ConcurrentQueue<User> _joinedUsers;

        private NetConnection _connection;
        private ConcurrentQueue<NetConnection> _leftConnection;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _connections = new List<NetConnection>();
            _joinedUsers = new ConcurrentQueue<User>();
            _leftConnection = new ConcurrentQueue<NetConnection>();

            provider.Service(out _userConnections);
            provider.Service(out _server);

            this.Users.OnAdded += this.HandleUserJoined;
            this.Users.OnRemoved += this.HandleUserLeft;
        }

        protected override void Release()
        {
            base.Release();

            _userConnections = null;
            _server = null;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            while(_joinedUsers.Any())
                if(_joinedUsers.TryDequeue(out _user))
                    _connections.Add(_userConnections.Connections[_user]);

            while (_leftConnection.Any())
                if (_leftConnection.TryDequeue(out _connection))
                    _connections.Remove(_connection);

            base.Update(gameTime);
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
            _joinedUsers.Enqueue(user);

            // Notify all connected users about the new in group user...
            this.Messages.Create(NetDeliveryMethod.ReliableOrdered, 0).Write(GuppyNetworkConstants.MessageTypes.UserJoined, om =>
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
            _leftConnection.Enqueue(_userConnections.Connections[user]);

            // Alert all remaining users if any about the user leaving
            this.Messages.Create(NetDeliveryMethod.ReliableOrdered, 0).Write(GuppyNetworkConstants.MessageTypes.UserLeft, om =>
            {
                om.Write(user.Id);
            });            
        }
        #endregion
    }
}
