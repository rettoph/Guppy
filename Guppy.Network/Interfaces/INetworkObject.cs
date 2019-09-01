using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface INetworkObject
    {
        void Read(NetIncomingMessage im);
        void Write(NetOutgoingMessage om);
    }
}
