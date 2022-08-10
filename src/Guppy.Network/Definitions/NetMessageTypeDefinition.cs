using Guppy.Network.Providers;
using Guppy.Network.Structs;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions
{
    public abstract class NetMessageTypeDefinition
    {
        public abstract Type Header { get; }
        public abstract DeliveryMethod DeliveryMethod { get; }
        public abstract byte OutgoingChannel { get; }

        internal abstract NetMessageType BuildType(NetId id, INetSerializerProvider serializers, INetDatumProvider data);
    }

    public abstract class NetMessageTypeDefinition<THeader> : NetMessageTypeDefinition
    {
        public override Type Header => typeof(THeader);

        internal override NetMessageType BuildType(NetId id, INetSerializerProvider serializers, INetDatumProvider data)
        {
            return new NetMessageType<THeader>(id, this.Header, this.DeliveryMethod, this.OutgoingChannel, serializers, data);
        }
    }
}
