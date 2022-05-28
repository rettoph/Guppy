using Guppy.Network.Providers;
using Guppy.Network.Structs;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetMessageFactories
{
    internal sealed class RuntimeNetMessageFactoryDefinition<T> : NetMessageFactoryDefinition<T>
    {
        public override DeliveryMethod DeliveryMethod { get; }

        public override byte OutgoingChannel { get; }

        public override int OutgoingPriority { get; }


        public RuntimeNetMessageFactoryDefinition(DeliveryMethod deliveryMethod, byte outgoingChannel, int outgoingPriority)
        {
            this.DeliveryMethod = deliveryMethod;
            this.OutgoingChannel = outgoingChannel;
            this.OutgoingPriority = outgoingPriority;
        }
    }
}
