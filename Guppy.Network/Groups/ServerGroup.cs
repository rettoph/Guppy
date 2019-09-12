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

namespace Guppy.Network.Groups
{
    public sealed class ServerGroup : Group
    {
        #region Constructor
        public ServerGroup(CreatableCollection<User> users, Peer peer) : base(users, peer)
        {
        }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize()
        {
            base.Initialize();

            this.Users.Events.TryAdd<User>("added", this.HandleUserAdded);
            this.Users.Events.TryAdd<User>("removed", this.HandleUserRemoved);
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
            // 1. Send the current user to all existing users
            NetOutgoingMessage om = this.CreateMessage("user:joined", NetDeliveryMethod.ReliableOrdered, 0);
            newUser.TryWrite(om);

            // 2. Send a list of all existing users to the new user
            this.Users.ForEach(user =>
            {
                if (user.Id != newUser.Id)
                { // Dont double send the new user their own data
                    om = this.CreateMessage("user:joined", newUser, NetDeliveryMethod.ReliableOrdered, 0);
                    user.TryWrite(om);
                }
            });

            // 3. Add the new user to the connections list
            if (newUser.Connection != default(NetConnection))
                this.connections.Add(newUser.Connection);
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
            var om = this.CreateMessage("user:left", NetDeliveryMethod.ReliableOrdered, 0);
            om.Write(oldUser.Id);

            // 2. Remove the users connection
            this.connections.Remove(oldUser.Connection);
        }
        #endregion
    }
}
