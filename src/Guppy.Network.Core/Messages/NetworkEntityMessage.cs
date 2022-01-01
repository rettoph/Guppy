using Guppy.Network.Interfaces;
using Guppy.Threading.Interfaces;
using LiteNetLib.Utils;
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
        public UInt16 NetworkId { get; internal set; }
        public List<IData> Packets { get; } = new List<IData>();

        protected internal virtual void Read(NetDataReader im, NetworkProvider network)
        {
            this.NetworkId = im.GetUShort();

            im.GetPackets(network, this);
        }

        protected internal virtual void Write(NetDataWriter om, NetworkProvider network)
        {
            om.Put(this.NetworkId);

            om.PutPackets(network, this);
        }
    }

    public abstract class NetworkEntityMessage<TNetworkEntityMessage> : NetworkEntityMessage
        where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
    {

    }
}
