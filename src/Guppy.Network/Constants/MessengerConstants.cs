using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Constants
{
    public static class MessengerConstants
    {
        public const DeliveryMethod PeerDeliveryMethod = DeliveryMethod.ReliableOrdered;
        public const int PeerOutgoingChannel = 0;
        public const int PeerOutgoingPriority = int.MinValue;
    }
}
