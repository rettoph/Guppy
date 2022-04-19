using Guppy.Network.Security;
using Guppy.Services.Common;
using LiteNetLib;
using Minnow.Collections;

namespace Guppy.Network.Services
{
    public sealed class RoomUserService : CollectionService<int, User>
    {
        private int _capacity;
        private List<NetPeer> _peers;

        public readonly Room Room;
        public IEnumerable<NetPeer> NetPeers => _peers;

        public event OnEventDelegate<RoomUserService, User>? OnUserJoined;
        public event OnEventDelegate<RoomUserService, User>? OnUserLeft;

        internal RoomUserService(Room room, int capacity) : base(capacity)
        {
            _capacity = capacity;
            _peers = new List<NetPeer>(capacity);

            this.Room = room;
        }

        protected override int GetId(User item)
        {
            return item.Id;
        }

        public bool TryJoin(User user)
        {
            if(this.items.Count < _capacity && this.items.TryAdd(this.GetId(user), user))
            {
                user.AddToRoom(this.Room);

                if(user.NetPeer is not null)
                {
                    _peers.Add(user.NetPeer);
                }

                this.OnUserJoined?.Invoke(this, user);
                return true;
            }

            return false;
        }

        public bool TryLeave(User user)
        {
            if(this.items.Remove(this.GetId(user)))
            {
                if(user.NetPeer is not null)
                {
                    _peers.Remove(user.NetPeer);
                }

                user.RemoveFromRoom(this.Room);

                this.OnUserLeft?.Invoke(this, user);
                return true;
            }

            return false;
        }
    }
}
