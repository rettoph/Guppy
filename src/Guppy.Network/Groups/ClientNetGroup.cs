using Guppy.Network.Enums;
using Guppy.Network.Identity;
using Guppy.Network.Messages;

namespace Guppy.Network.Groups
{
    internal sealed class ClientNetGroup : BaseNetGroup
    {
        public ClientNetGroup(byte id, IPeer peer) : base(id, peer)
        {
        }

        protected override void Process(INetIncomingMessage<UserAction> message)
        {
            base.Process(message);

            User user = Peer.Users.UpdateOrCreate(message.Body.UserDto);

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
