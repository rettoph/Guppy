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

            User user = Peer.Users.UpdateOrCreate(message.Body.Id, message.Body.Claims);

            switch (message.Body.Type)
            {
                case UserActionTypes.UserJoined:
                    Users.Add(user);
                    break;
                case UserActionTypes.UserLeft:
                    Users.Remove(user);
                    break;
            }
        }
    }
}
