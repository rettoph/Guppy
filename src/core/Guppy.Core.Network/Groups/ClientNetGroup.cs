using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Messages;
using Guppy.Core.Network.Common.Peers;

namespace Guppy.Core.Network.Common.Groups
{
    internal sealed class ClientNetGroup : BaseNetGroup
    {
        public ClientNetGroup(byte id, IPeer peer) : base(id, peer)
        {
        }

        public override void Process(INetIncomingMessage<UserAction> message)
        {
            base.Process(message);

            IUser user = this.Peer.Users.UpdateOrCreate(message.Body.UserDto);

            switch (message.Body.Type)
            {
                case UserActionTypes.UserJoined:
                    this.Users.Add(user);
                    break;
                case UserActionTypes.UserLeft:
                    this.Users.Remove(user);
                    break;
            }
        }
    }
}
