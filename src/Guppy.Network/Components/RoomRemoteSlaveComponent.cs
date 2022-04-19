using Guppy.EntityComponent;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Security.Services;
using Guppy.Network.Services;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Slave)]
    internal sealed class RoomRemoteSlaveComponent : Component, ISubscriber<NetIncomingMessage<UserActionMessage>>
    {
        private readonly IUserService _users;

        public readonly Room Room;

        public RoomRemoteSlaveComponent(IUserService users, Room room)
        {
            _users = users;

            this.Room = room;

            this.Room.Messages.OnBusChanged += this.UpdateMessageBus;
            this.UpdateMessageBus(this.Room.Messages, null, this.Room.Messages.Bus);
        }

        public bool Process(in NetIncomingMessage<UserActionMessage> message)
        {
            var user = _users.UpdateOrCreate(message.Content.Instance.Id, message.Content.Instance.Claims);

            switch (message.Content.Instance.Type)
            {
                case UserActionType.Joined:
                    return this.Room.Users.TryJoin(user);
                case UserActionType.Left:
                    return this.Room.Users.TryLeave(user);
                default:
                    throw new InvalidOperationException();
            }
        }

        private void UpdateMessageBus(RoomMessageService sender, Bus? old, Bus? value)
        {
            if(old is not null)
            {
                old.Unsubscribe<NetIncomingMessage<UserActionMessage>>(this);
            }

            if(value is not null)
            {
                value.Subscribe<NetIncomingMessage<UserActionMessage>>(this);
            }
        }
    }
}
