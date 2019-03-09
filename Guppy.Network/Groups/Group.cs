using Guppy.Network.Collections;
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
        #region Public Attributes
        /// <summary>
        /// The current group id
        /// </summary>
        public Int32 Id { get; private set; }

        /// <summary>
        /// The known users in the current game
        /// </summary>
        public UserCollection Users { get; private set; }
        #endregion

        #region Constructor
        public Group(Int32 id)
        {
            this.Id = id;
        }
        #endregion
    }
}
