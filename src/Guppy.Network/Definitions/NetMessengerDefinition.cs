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
    public abstract class NetMessengerDefinition
    {
        public abstract Type Type { get; }
        public abstract DeliveryMethod DeliveryMethod { get; }
        public abstract byte OutgoingChannel { get; }
        public abstract int OutgoingPriority { get; }

        public abstract NetMessenger BuildNetMessenger(DynamicId id, INetSerializerProvider serializers);
    }

    public abstract class NetMessengerDefinition<T> : NetMessengerDefinition
    {
        public override Type Type { get; } = typeof(T);

        public override NetMessenger BuildNetMessenger(DynamicId id, INetSerializerProvider serializers)
        {
            return new NetMessenger<T>(id, this.DeliveryMethod, this.OutgoingChannel, this.OutgoingPriority, serializers);
        }
    }
}
