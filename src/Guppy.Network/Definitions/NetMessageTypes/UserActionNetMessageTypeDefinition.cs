using Guppy.Attributes;
using Guppy.Network.Constants;
using Guppy.Network.Messages;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetMessageTypes
{
    [AutoLoad]
    internal sealed class UserActionNetMessageTypeDefinition : NetMessageTypeDefinition<UserAction>
    {
        public override DeliveryMethod DefaultDeliveryMethod => DeliveryMethod.ReliableOrdered;

        public override byte DefaultOutgoingChannel => PeerConstants.OutgoingChannel;
    }
}
