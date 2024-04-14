using Guppy.Engine.Attributes;
using Guppy.Engine.Enums;
using Guppy.Network.Services;
using LiteNetLib;

namespace Guppy.Network.Definitions
{
    [Service<NetMessageTypeDefinition>(ServiceLifetime.Singleton, true)]
    public abstract class NetMessageTypeDefinition
    {
        public abstract Type Body { get; }
        public abstract DeliveryMethod DefaultDeliveryMethod { get; }
        public abstract byte DefaultOutgoingChannel { get; }

        internal abstract NetMessageType Build(byte id, IPeer peer, INetSerializerService serializers);
    }

    public abstract class NetMessageTypeDefinition<TBody> : NetMessageTypeDefinition
        where TBody : notnull
    {
        public override Type Body => typeof(TBody);

        internal override NetMessageType Build(byte id, IPeer peer, INetSerializerService serializers)
        {
            return new NetMessageType<TBody>(id, this.Body, this.DefaultDeliveryMethod, this.DefaultOutgoingChannel, peer, serializers);
        }
    }
}
