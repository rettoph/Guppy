using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface INetworkObject : IUniqueObject
    {
        Guid Id { get; }
        Boolean Dirty { get; set; }
        event EventHandler<INetworkObject> OnDirtyChanged;

        void Read(NetIncomingMessage im);
        void Write(NetOutgoingMessage om);
    }
}
