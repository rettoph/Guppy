using Guppy.Network.Providers;
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
        public abstract Type Body { get; }
        public abstract DeliveryMethod DeliveryMethod { get; }
        public abstract byte OutgoingChannel { get; }

        internal abstract NetMessageType BuildType(INetId id, INetSerializerProvider serializers, INetDatumProvider data);
    }

    public abstract class NetMessageTypeDefinition<TBody> : NetMessageTypeDefinition
    {
        public override Type Body => typeof(TBody);

        internal override NetMessageType BuildType(INetId id, INetSerializerProvider serializers, INetDatumProvider data)
        {
            return new NetMessageType<TBody>(id, this.Body, this.DeliveryMethod, this.OutgoingChannel, serializers, data);
        }
    }
}
