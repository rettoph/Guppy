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
        public abstract DeliveryMethod DefaultDeliveryMethod { get; }
        public abstract byte DefaultOutgoingChannel { get; }

        internal abstract NetMessageType BuildType(INetId id, INetSerializerProvider serializers, NetScope scope);
    }

    public abstract class NetMessageTypeDefinition<TBody> : NetMessageTypeDefinition
        where TBody : notnull
    {
        public override Type Body => typeof(TBody);

        internal override NetMessageType BuildType(INetId id, INetSerializerProvider serializers, NetScope scope)
        {
            return new NetMessageType<TBody>(id, this.Body, this.DefaultDeliveryMethod, this.DefaultOutgoingChannel, serializers, scope);
        }
    }
}
