using Guppy.Network.Collections;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Groups
{
    public class ServerGroup : Group
    {
        public ServerGroup(int id, Peer peer) : base(id, peer)
        {
            this.Users.Added += this.HandleUserAdded;
            this.Users.Removed += this.HandleUserRemoved;
        }

        #region Event Handlers
        private void HandleUserAdded(object sender, User user)
        {
            // Send an update message to all contained users
        }

        private void HandleUserRemoved(object sender, User user)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
