using Guppy.Network.Constants;
using Guppy.Network.Definitions;
using Guppy.Network.Security.Messages;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Definitions.NetMessengers
{
    internal sealed class ConnectionResponseMessageNetMessengerDefinition : NetMessengerDefinition<ConnectionResponseMessage>
    {
        public override DeliveryMethod DeliveryMethod => MessengerConstants.PeerDeliveryMethod;

        public override byte OutgoingChannel => MessengerConstants.PeerOutgoingChannel;

        public override int OutgoingPriority => MessengerConstants.PeerOutgoingPriority;
    }
}
