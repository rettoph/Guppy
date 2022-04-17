using Guppy.EntityComponent;
using Guppy.Network.Providers;
using Guppy.Network.Services;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public sealed class Room : Entity
    {
        private INetMessengerProvider _messengers;

        public new readonly byte Id;
        public readonly RoomMessageService Messages;
        public readonly RoomUserService Users;

        public Room(byte id, INetMessengerProvider messengers)
        {
            _messengers = messengers;

            this.Id = id;
            this.Messages = new RoomMessageService(_messengers, this);
            this.Users = new RoomUserService(this);
        }
    }
}
