using Guppy.EntityComponent;
using Guppy.Network.Constants;
using Guppy.Network.Providers;
using Guppy.Network.Services;
using Guppy.Providers;
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
        private readonly INetMessengerProvider _messengers;
        private readonly ISettingProvider _settings;

        public new readonly byte Id;
        public readonly RoomMessageService Messages;
        public readonly RoomUserService Users;

        public Room(byte id, INetMessengerProvider messengers, ISettingProvider settings)
        {
            _messengers = messengers;
            _settings = settings;

            this.Id = id;
            this.Messages = new RoomMessageService(_messengers, this);
            this.Users = new RoomUserService(this, _settings.Get<int>(SettingConstants.MaxRoomUsers).Value);
        }
    }
}
