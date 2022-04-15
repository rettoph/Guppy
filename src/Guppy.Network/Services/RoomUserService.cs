using Guppy.Network.Security;
using Guppy.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public sealed class RoomUserService : CollectionService<int, User>
    {
        public readonly Room Room;

        public event OnEventDelegate<RoomUserService, User>? OnUserJoined;
        public event OnEventDelegate<RoomUserService, User>? OnUserLeft;

        internal RoomUserService(Room room)
        {
            this.Room = room;
        }

        protected override int GetId(User item)
        {
            return item.Id;
        }

        public bool TryJoin(User user)
        {
            if(this.items.TryAdd(this.GetId(user), user))
            {
                user.AddToRoom(this.Room);

                this.OnUserJoined?.Invoke(this, user);
                return true;
            }

            return false;
        }

        public bool TryLeave(User user)
        {
            if(this.items.Remove(this.GetId(user)))
            {
                user.RemoveFromRoom(this.Room);

                this.OnUserLeft?.Invoke(this, user);
                return true;
            }

            return false;
        }
    }
}
