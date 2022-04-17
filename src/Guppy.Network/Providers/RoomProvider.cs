using Guppy.EntityComponent.Services;
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
        private IEntityService _entities;

        private Dictionary<int, Room> _rooms;

        public RoomProvider(INetMessengerProvider messengers, IEntityService entities)
        {
            _messengers = messengers;
            _entities = entities;
            _rooms = new Dictionary<int, Room>();
        }

        public Room Get(in byte id)
        {
            if(!_rooms.TryGetValue(id, out Room? room))
            {
                _rooms.Add(id, room = new Room(id, _messengers));
                _entities.TryAdd(room);
            }

            return room;
        }

        public void ProcessIncoming(NetIncomingMessage message)
        {
            this.Get(message.RoomId).Messages.ProcessIncoming(message);
        }
    }
}
