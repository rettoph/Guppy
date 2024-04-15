using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Extensions.Identity;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;

namespace Guppy.Core.Network.Common.Groups
{
    internal sealed class ServerNetGroup : BaseNetGroup
    {
        public ServerNetGroup(byte id, IPeer peer) : base(id, peer)
        {
            this.Users.Add(peer.Users.Current!);

            this.Users.OnUserJoined += HandleUserJoined;
            this.Users.OnUserLeft += HandleUserLeft;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Users.OnUserJoined -= HandleUserJoined;
            this.Users.OnUserLeft -= HandleUserLeft;
        }

        private void HandleUserJoined(INetScopeUserService sender, IUser newUser)
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
            foreach (IUser oldUser in this.Users)
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

        private void HandleUserLeft(INetScopeUserService sender, IUser user)
        {
            this.CreateMessage(user.CreateAction(UserActionTypes.UserLeft, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();
        }
    }
}
