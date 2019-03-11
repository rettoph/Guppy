using Guppy.Network.Collections;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Groups
{
    /// <summary>
    /// Groups represent a collection of users who can privately
    /// send messages to other users within the same group.
    /// </summary>
    public class Group
    {
        #region Private Methods
        private Peer _peer;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The current group id
        /// </summary>
        public Int32 Id { get; private set; }

        /// <summary>
        /// The known users in the current game
        /// </summary>
        public NetworkObjectCollection<User> Users { get; private set; }
        #endregion

        #region Constructor
        public Group(Int32 id, Peer peer)
        {
            this.Id = id;
            this.Users = new NetworkObjectCollection<User>();

            _peer = peer;
        }
        #endregion

        #region Create Message Methods
        protected internal NetOutgoingMessage CreatMessage(MessageType type = MessageType.Data)
        {
            var om = _peer.CreateMessage(MessageTarget.Group);
            om.Write(this.Id);
            om.Write((Byte)type);

            return om;
        }
        #endregion
    }
}
