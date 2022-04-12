using Guppy.Network.Providers;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    internal sealed class NetMessengerCollection : List<NetMessengerDescriptor>, INetMessengerCollection
    {
        public INetMessengerCollection Add<T>(DeliveryMethod deliveryMethod, byte outgoingChannel, int outgoingPriority)
        {
            this.Add(NetMessengerDescriptor.Create<T>(deliveryMethod, outgoingChannel, outgoingPriority));

            return this;
        }

        public INetMessengerProvider Build(INetSerializerProvider serializers)
        {
            return new NetMessengerProvider(serializers, this);
        }
    }
}
