using Guppy.Network.Interfaces;
using Guppy.Threading.Interfaces;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages
{
    public abstract class NetworkEntityMessage : IData
    {
        public UInt16 NetworkId { get; internal init; }
        public List<IPacket> Packets { get; } = new List<IPacket>();
    }

    public abstract class NetworkEntityMessage<TNetworkEntityMessage> : NetworkEntityMessage
        where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
    {

    }
}
