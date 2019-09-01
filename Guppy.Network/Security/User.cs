using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security
{
    public sealed class User : Initializable, INetworkObject
    {
        #region Internal Attributes
        internal NetConnection Connection { get; set; }
        #endregion

        #region Public Attributes
        /// <summary>
        /// The current user's name.
        /// </summary>
        public String Name { get; internal set; }

        /// <summary>
        /// Represents whether or not the user has been
        /// verified by the server.
        /// </summary>
        public Boolean Verified { get; internal set; }
        #endregion

        #region INetworkObject Implmentation
        public void Read(NetIncomingMessage im)
        {
            this.Name = im.ReadString();
            this.Verified = im.ReadBoolean();
        }

        public void Write(NetOutgoingMessage om)
        {
            om.Write(this.Name);
            om.Write(this.Verified);
        }
        #endregion
    }
}
