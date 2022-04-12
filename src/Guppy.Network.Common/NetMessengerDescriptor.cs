using Guppy.Network.Providers;
using Guppy.Network.Structs;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetMessengerDescriptor
    {
        public readonly DeliveryMethod DeliveryMethod;
        public readonly byte OutgoingChannel;
        public readonly int OutgoingPriority;

        protected NetMessengerDescriptor(DeliveryMethod deliveryMethod, byte outgoingChannel, int outgoingPriority)
        {
            this.DeliveryMethod = deliveryMethod;
            this.OutgoingChannel = outgoingChannel;
            this.OutgoingPriority = outgoingPriority;
        }

        public abstract NetMessenger Create(DynamicId id, INetSerializerProvider serializers);

        public static NetMessengerDescriptor Create<T>(
            DeliveryMethod deliveryMethod,
            byte outgoingChannel,
            int outgoingPriority)
        {
            return new NetMessengerDescriptor<T>(deliveryMethod, outgoingChannel, outgoingPriority);
        }
    }

    internal sealed class NetMessengerDescriptor<T> : NetMessengerDescriptor
    {
        public NetMessengerDescriptor(
            DeliveryMethod deliveryMethod, 
            byte outgoingChannel, 
            int outgoingPriority) : base(deliveryMethod, outgoingChannel, outgoingPriority)
        {
        }

        public override NetMessenger Create(DynamicId id, INetSerializerProvider serializers)
        {
            return new NetMessenger<T>(
                id,
                this.DeliveryMethod,
                this.OutgoingChannel,
                this.OutgoingPriority,
                serializers);
        }
    }
}
