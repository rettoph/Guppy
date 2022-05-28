using Guppy.Network.Constants;
using Guppy.Network.Messages;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetMessageFactories
{
    internal sealed class NetTargetActionMessageNetMessageFactoryDefinition : NetMessageFactoryDefinition<NetTargetActionMessage>
    {
        public override DeliveryMethod DeliveryMethod => MessengerConstants.PeerDeliveryMethod;

        public override byte OutgoingChannel => MessengerConstants.PeerOutgoingChannel;

        public override int OutgoingPriority => MessengerConstants.PeerOutgoingPriority;
    }
}
