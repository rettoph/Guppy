using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetMessageTypes
{
    internal sealed class RuntimeNetMessageTypeDefinition<T> : NetMessageTypeDefinition<T>
    {
        public RuntimeNetMessageTypeDefinition(DeliveryMethod deliveryMethod, byte outgoingChannel)
        {
            this.DeliveryMethod = deliveryMethod;
            this.OutgoingChannel = outgoingChannel;
        }

        public override DeliveryMethod DeliveryMethod { get; }

        public override byte OutgoingChannel { get; }
    }
}
