using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Interfaces;
using Guppy.Implementations;

namespace Guppy.Network
{
    public class NetworkObject : Initializable
    {
        #region Constructors
        public NetworkObject(IServiceProvider provider) : base(provider)
        {
        }

        public NetworkObject(Guid id, IServiceProvider provider) : base(id, provider)
        {
        }
        #endregion

        public virtual void Read(NetIncomingMessage im)
        {
            //
        }

        public virtual void Write(NetOutgoingMessage om)
        {
            // Write the network object's id
            om.Write(this.Id);
        }
    }
}
