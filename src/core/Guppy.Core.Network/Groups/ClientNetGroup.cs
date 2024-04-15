using Guppy.Core.Network.Enums;
using Guppy.Core.Network.Identity;
using Guppy.Core.Network.Messages;

namespace Guppy.Core.Network.Groups
{
    internal sealed class ClientNetGroup : BaseNetGroup
    {
        public ClientNetGroup(byte id, IPeer peer) : base(id, peer)
        {
        }

        protected override void Process(INetIncomingMessage<UserAction> message)
        {
            base.Process(message);

            User user = this.Peer.Users.UpdateOrCreate(message.Body.UserDto);

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
