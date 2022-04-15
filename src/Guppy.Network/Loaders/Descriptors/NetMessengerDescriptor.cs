using Guppy.Network.Providers;
using Guppy.Network.Structs;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders.Descriptors
{
    public abstract class NetMessengerDescriptor
    {
        public readonly Type Type;
        public readonly DeliveryMethod DeliveryMethod;
        public readonly byte OutgoingChannel;
        public readonly int OutgoingPriority;

        protected NetMessengerDescriptor(Type type, DeliveryMethod deliveryMethod, byte outgoingChannel, int outgoingPriority)
        {
            this.Type = type;
            this.DeliveryMethod = deliveryMethod;
            this.OutgoingChannel = outgoingChannel;
            this.OutgoingPriority = outgoingPriority;
        }

        public abstract NetMessenger BuildNetMessenger(DynamicId id, INetSerializerProvider serializers);

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
            int outgoingPriority) : base(typeof(T), deliveryMethod, outgoingChannel, outgoingPriority)
        {
        }

        public override NetMessenger BuildNetMessenger(DynamicId id, INetSerializerProvider serializers)
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
