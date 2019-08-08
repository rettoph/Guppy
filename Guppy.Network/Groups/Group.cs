using Guppy.Implementations;
using Guppy.Network.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Implementations;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Groups
{
    /// <summary>
    /// A group is a collection of users that
    /// recieve specific messages. A user must be
    /// added to a group by the server at which point
    /// that user will recieve all group messages and
    /// can send messages via the group to the server.
    /// </summary>
    public abstract class Group : Target
    {
        #region Public Attributes
        public UserCollection Users { get; private set; }
        #endregion

        #region Constructor
        public Group(UserCollection users, NetPeer peer, Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool) : base(peer, outgoingMessageConfigurationPool)
        {
            this.Users = users;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Users.Clear();
        }
        #endregion
    }
}
