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
        private static Factory<List<IPacket>> PacketListFactory = new Factory<List<IPacket>>(() => new List<IPacket>(), 50);

        public UInt16 NetworkId { get; internal init; }
        public List<IPacket> Packets { get; } = PacketListFactory.GetInstance();

        #region IData Implementation
        void IData.Clean()
        {
            foreach(IPacket packet in this.Packets)
            {
                packet.Clean();
            }

            this.Packets.Clear();

            PacketListFactory.TryReturnToPool(this.Packets);
        }
        #endregion
    }

    public abstract class NetworkEntityMessage<TNetworkEntityMessage> : NetworkEntityMessage
        where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
    {

    }
}
