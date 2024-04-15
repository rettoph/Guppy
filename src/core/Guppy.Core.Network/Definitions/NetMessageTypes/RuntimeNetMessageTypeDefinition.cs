using LiteNetLib;

namespace Guppy.Core.Network.Definitions.NetMessageTypes
{
    internal sealed class RuntimeNetMessageTypeDefinition<T> : NetMessageTypeDefinition<T>
        where T : notnull
    {
        public RuntimeNetMessageTypeDefinition(DeliveryMethod deliveryMethod, byte outgoingChannel)
        {
            this.DefaultDeliveryMethod = deliveryMethod;
            this.DefaultOutgoingChannel = outgoingChannel;
        }

        public override DeliveryMethod DefaultDeliveryMethod { get; }

        public override byte DefaultOutgoingChannel { get; }
    }
}
