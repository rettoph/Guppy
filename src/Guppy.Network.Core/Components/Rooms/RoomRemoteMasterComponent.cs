using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.EventArgs;
using Guppy.Network.Security.Lists;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components.Rooms
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    internal class RoomRemoteMasterComponent : Component<Room>
    {
        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Entity.Users.OnUserAdded += this.HandleUserAdded;
            this.Entity.Users.OnUserRemoved += this.HandleUserRemoved;
        }
        #endregion

        #region Helper Methods
        private void SendUserRoomActionMessage(UserRoomAction action, User user, NetPeer recipient)
        {
            // First, broadcast the new user to all connected users.
            this.Entity.SendMessage(new UserRoomActionMessage()
            {
                Action = action,
                RoomId = this.Entity.Id,
                User = user.GetDto(ClaimType.Public)
            });

            // Second, broadcast all connected users to the connected user.
            // TODO: Maybe dont be lazy and do this properly? Just an idea...
            foreach (User existingUser in this.Entity.Users.Except(user.Yield()))
            {
                this.Entity.SendMessage(new UserRoomActionMessage()
                {
                    Action = action,
                    RoomId = this.Entity.Id,
                    User = existingUser.GetDto(ClaimType.Public)
                }, recipient);
            }
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(UserList sender, UserEventArgs args)
        {
            this.SendUserRoomActionMessage(UserRoomAction.Joined, args.User, args.User.NetPeer);
        }

        private void HandleUserRemoved(UserList sender, UserEventArgs args)
        {
            this.SendUserRoomActionMessage(UserRoomAction.Left, args.User, args.User.NetPeer);
        }
        #endregion
    }
}
