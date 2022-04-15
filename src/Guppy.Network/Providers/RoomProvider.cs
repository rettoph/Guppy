using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal sealed class RoomProvider : IRoomProvider
    {
        private INetMessengerProvider _messengers;
        private Dictionary<int, Room> _rooms;

        public RoomProvider(INetMessengerProvider messengers)
        {
            _messengers = messengers;
            _rooms = new Dictionary<int, Room>();
        }

        public Room Get(in byte id)
        {
            if(!_rooms.TryGetValue(id, out Room? room))
            {
                _rooms.Add(id, room = new Room(id, _messengers));
            }

            return room;
        }

        public void ProcessIncoming(NetIncomingMessage message)
        {
            this.Get(message.RoomId).Messages.ProcessIncoming(message);
        }
    }
}
