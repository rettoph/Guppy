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
        public String Name { get; internal set; }
        #endregion

        #region INetworkObject Implmentation
        public void Read(NetIncomingMessage im)
        {
            this.Name = im.ReadString();
        }

        public void Write(NetOutgoingMessage om)
        {
            om.Write(this.Name);
        }
        #endregion
    }
}
