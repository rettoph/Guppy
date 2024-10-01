using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using LiteNetLib;

namespace Guppy.Core.Network.Common.Definitions
{
    [Service<NetMessageTypeDefinition>(ServiceLifetime.Singleton, ServiceRegistrationFlags.RequireAutoLoadAttribute)]
    public abstract class NetMessageTypeDefinition
    {
        public abstract Type Body { get; }
        public abstract DeliveryMethod DefaultDeliveryMethod { get; }
        public abstract byte DefaultOutgoingChannel { get; }
    }

    public class NetMessageTypeDefinition<TBody>(DeliveryMethod deliveryMethod, byte outgoingChannel) : NetMessageTypeDefinition
        where TBody : notnull
    {
        public override Type Body => typeof(TBody);

        public override DeliveryMethod DefaultDeliveryMethod { get; } = deliveryMethod;

        public override byte DefaultOutgoingChannel { get; } = outgoingChannel;
    }
}
