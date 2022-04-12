using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public interface INetMessengerCollection : IList<NetMessengerDescriptor>
    {
        INetMessengerCollection Add<T>(
            DeliveryMethod deliveryMethod,
            byte outgoingChannel,
            int outgoingPriority);
    }
}
