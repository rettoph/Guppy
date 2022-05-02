using Guppy.EntityComponent;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using Guppy.Network.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    internal sealed class RoomRemoteMasterComponent : Component
    {
        public readonly Room Room;

        public RoomRemoteMasterComponent(Room room)
        {
            this.Room = room;

            this.Room.Users.OnUserJoined += this.HandleUserJoined;
            this.Room.Users.OnUserLeft += this.HandleUserLeft;
        }

        private void HandleUserLeft(RoomUserService sender, User oldUser)
        {
            // Broadcast the old user to all existing connections...
            this.Room.Messages.CreateOutgoing<UserActionMessage>(UserActionMessage.Left(oldUser)).EnqueueSend();
        }

        private void HandleUserJoined(RoomUserService sender, User newUser)
        {
            // Broadcast the new user to all existing connections...
            this.Room.Messages.CreateOutgoing<UserActionMessage>(UserActionMessage.Joined(newUser)).EnqueueSend();

            
            if (newUser.NetPeer is null)
            {
                return;
            }

            // Broadcast all existing users to the new connection...
            foreach (User user in this.Room.Users)
            {
                if(user != newUser)
                {
                    this.Room.Messages.CreateOutgoing<UserActionMessage>(UserActionMessage.Joined(user), newUser.NetPeer).EnqueueSend();
                }
            }
        }
    }
}
