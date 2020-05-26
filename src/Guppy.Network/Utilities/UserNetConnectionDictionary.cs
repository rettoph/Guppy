using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// A lookup table useful for finding the netconnection
    /// of a user instance of vice versa. This is only maintained
    /// within a server peer.
    /// </summary>
    public sealed class UserNetConnectionDictionary
    {
        private Dictionary<User, NetConnection> _connections;
        private Dictionary<NetConnection, User> _users;

        public IReadOnlyDictionary<User, NetConnection> Connections => _connections;
        public IReadOnlyDictionary<NetConnection, User> Users => _users;

        internal UserNetConnectionDictionary()
        {
            _connections = new Dictionary<User, NetConnection>();
            _users = new Dictionary<NetConnection, User>();
        }

        internal void Add(User user, NetConnection connection)
        {
            _connections.Add(user, connection);
            _users.Add(connection, user);

            user.OnDisposed += this.HandleUserDisposed;
        }

        private void HandleUserDisposed(IService sender)
        {
            var user = sender as User;

            _users.Remove(_connections[user]);
            _connections.Remove(user);

            user.OnDisposed -= this.HandleUserDisposed;
        }
    }
}
