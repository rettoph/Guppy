using Guppy.Network.Messages;

namespace Guppy.Network.Groups
{
    internal sealed class ServerNetGroup : BaseNetGroup
    {
        public ServerNetGroup(byte id, IPeer peer) : base(id, peer)
        {
        }

        protected override void Process(INetIncomingMessage<UserAction> message)
        {
            base.Process(message);
        }
    }
}
