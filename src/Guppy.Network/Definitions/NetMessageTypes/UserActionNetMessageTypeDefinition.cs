using Guppy.Attributes;
using Guppy.Network.Constants;
using Guppy.Network.Messages;
using LiteNetLib;

namespace Guppy.Network.Definitions.NetMessageTypes
{
    [AutoLoad]
    internal sealed class UserActionNetMessageTypeDefinition : NetMessageTypeDefinition<UserAction>
    {
        public override DeliveryMethod DefaultDeliveryMethod => DeliveryMethod.ReliableOrdered;

        public override byte DefaultOutgoingChannel => PeerConstants.OutgoingChannel;
    }
}
