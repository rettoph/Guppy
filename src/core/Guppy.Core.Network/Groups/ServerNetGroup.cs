using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Extensions.Identity;

namespace Guppy.Core.Network.Groups
{
    internal sealed class ServerNetGroup : BaseNetGroup
    {
        public ServerNetGroup(byte id, IPeer peer) : base(id, peer)
        {
            this.Users.Add(peer.Users.Current!);

            this.Users.OnUserJoined += this.HandleUserJoined;
            this.Users.OnUserLeft += this.HandleUserLeft;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Users.OnUserJoined -= this.HandleUserJoined;
            this.Users.OnUserLeft -= this.HandleUserLeft;
        }

        private void HandleUserJoined(INetScopeUserService sender, IUser newUser)
        {
            // Alert all users of the new user.
            this.Peer.Group.CreateMessage(newUser.CreateAction(this.Id, UserActionTypeEnum.UserJoined, ClaimAccessibilityEnum.Public))
                .AddRecipients(this.Users.Peers)
                .Send();

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

                this.Peer.Group.CreateMessage(oldUser.CreateAction(this.Id, UserActionTypeEnum.UserJoined, ClaimAccessibilityEnum.Public))
                    .AddRecipient(newUser.NetPeer)
                    .Send();
            }
        }

        private void HandleUserLeft(INetScopeUserService sender, IUser user)
        {
            this.Peer.Group.CreateMessage(user.CreateAction(this.Id, UserActionTypeEnum.UserLeft, ClaimAccessibilityEnum.Public))
                .AddRecipients(this.Users.Peers)
                .Send();
        }
    }
}