using Guppy.Network.Constants;
using Guppy.Network.Messages;
using LiteNetLib;

namespace Guppy.Network.Definitions.NetMessengers
{
    internal sealed class UserActionMessageNetMessengerDefinition : NetMessengerDefinition<UserActionMessage>
    {
        public override DeliveryMethod DeliveryMethod => MessengerConstants.PeerDeliveryMethod;

        public override byte OutgoingChannel => MessengerConstants.PeerOutgoingChannel;

        public override int OutgoingPriority => MessengerConstants.PeerOutgoingPriority;
    }
}
