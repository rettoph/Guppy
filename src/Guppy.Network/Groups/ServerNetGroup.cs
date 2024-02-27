using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Services;

namespace Guppy.Network.Groups
{
    internal sealed class ServerNetGroup : BaseNetGroup
    {
        public ServerNetGroup(byte id, IPeer peer) : base(id, peer)
        {
            this.Users.OnUserJoined += HandleUserJoined;
            this.Users.OnUserLeft += HandleUserLeft;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Users.OnUserJoined -= HandleUserJoined;
            this.Users.OnUserLeft -= HandleUserLeft;
        }

        private void HandleUserJoined(INetScopeUserService sender, User newUser)
        {
            // Alert all users of the new user.
            this.CreateMessage(newUser.CreateAction(UserActionTypes.UserJoined, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();

            if (newUser.NetPeer is null)
            {
                return;
            }

            // Alert the new user of all existing users.
            foreach (User oldUser in this.Users)
            {
                if (oldUser.Id == newUser.Id)
                {
                    continue;
                }

                this.CreateMessage(oldUser.CreateAction(UserActionTypes.UserJoined, ClaimAccessibility.Public))
                    .AddRecipient(newUser.NetPeer)
                    .Enqueue();
            }
        }

        private void HandleUserLeft(INetScopeUserService sender, User user)
        {
            this.CreateMessage(user.CreateAction(UserActionTypes.UserLeft, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();
        }
    }
}
