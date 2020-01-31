using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using Guppy.Network.Extensions.Lidgren;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Guppy.Network.Groups
{
    public sealed class ServerGroup : Group
    {
        #region Private Fields
        private Queue<User> _newUsers;
        #endregion

        #region Internal Attributes
        protected internal override IList<NetConnection> connections { get; protected set; }
        #endregion

        #region Constructor
        public ServerGroup(ServerPeer server, CreatableCollection<User> users) : base(users, server)
        {
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.connections = new List<NetConnection>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _newUsers = new Queue<User>();

            this.Users.OnAdded += this.HandleUserAdded;
            this.Users.OnRemoved += this.HandleUserRemoved;
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            while (_newUsers.Any())
                this.FlushNewUserInfo(_newUsers.Dequeue());
        }
        #endregion

        #region Helper Methods
        private void FlushNewUserInfo(User newUser)
        {
            // 1. Send the current user to all existing users
            NetOutgoingMessage om = this.Messages.Create("user:joined", NetDeliveryMethod.ReliableOrdered, 0);
            newUser.TryWrite(om);

            // 2. Send a list of all existing users to the new user
            this.Users.ForEach(user =>
            {
                if (user.Id != newUser.Id)
                { // Dont double send the new user their own data
                    om = this.Messages.Create("user:joined", NetDeliveryMethod.ReliableOrdered, 0, newUser);
                    user.TryWrite(om);
                }
            });
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// When a user connects to a group several things must take place...
        /// 
        /// 1. We must send the new user a list of all previously connected users.
        /// 2. We must update all connected users of the new user
        /// 3. If the user has a connection defined, add it to the internal
        /// connections set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newUser"></param>
        private void HandleUserAdded(object sender, User newUser)
        {
            // Add the new user to the connections list
            if (newUser.Connection != default(NetConnection))
                this.connections.Add(newUser.Connection);

            _newUsers.Enqueue(newUser);
        }

        /// <summary>
        /// When a user disconnects from a group several things must happen...
        /// 
        /// 1. All connected users must be updated...
        /// 2. The users old connection must be removed...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="oldUser"></param>
        private void HandleUserRemoved(object sender, User oldUser)
        {
            // 1. Broadcast the removed user to all users
            var om = this.Messages.Create("user:left", NetDeliveryMethod.ReliableOrdered, 0);
            om.Write(oldUser.Id);

            // 2. Remove the users connection
            this.connections.Remove(oldUser.Connection);
        }
        #endregion
    }
}
