using Guppy.EntityComponent;
using Guppy.Network.Components;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Security;
using Guppy.Network.Security.Providers;
using Guppy.Network.Services;
using Guppy.Network.Utilities;
using Guppy.Providers;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    /// <summary>
    /// Simple internal entity used to process incoming/outgoing system messages.
    /// </summary>
    public sealed class Room : Entity, INetTarget, IInitializable,
        ISubscriber<NetIncomingMessage<UserActionMessage>>,
        ISubscriber<NetIncomingMessage<NetTargetActionMessage>>
    {
        private readonly RequestAuthorizer _authorizer;
        private readonly IUserProvider _users;

        public readonly RoomUserService Users;

        public Room(
            RequestAuthorizer authorizer,
            IUserProvider users,
            ISettingProvider settings)
        {
            _authorizer = authorizer;
            _users = users;

            this.Users = new RoomUserService(this, settings.Get<int>(SettingConstants.MaxRoomUsers).Value);
        }

        public void Initialize()
        {
            this.Components.Get<NetMessenger>().Subscribe<NetIncomingMessage<UserActionMessage>>(this);
            this.Components.Get<NetMessenger>().Subscribe<NetIncomingMessage<NetTargetActionMessage>>(this);

            this.Components.Get<NetMessenger>().Start(ushort.MaxValue);

            this.Users.OnUserJoined += this.HandleUserJoined;
            this.Users.OnUserLeft += this.HandleUserLeft;
        }

        public void Uninitialize()
        {
            this.Users.OnUserJoined -= this.HandleUserJoined;
            this.Users.OnUserLeft -= this.HandleUserLeft;

            this.Components.Get<Components.NetMessenger>().Unsubscribe<NetIncomingMessage<UserActionMessage>>(this);
        }

        bool ISubscriber<NetIncomingMessage<UserActionMessage>>.Process(in NetIncomingMessage<UserActionMessage> message)
        {
            if(message.Content.Instance is null)
            {
                return false;
            }

            if(!_authorizer.Authorize(NetworkAuthorization.Slave, message))
            {
                var user = _users.UpdateOrCreate(message.Content.Instance.Id, message.Content.Instance.Claims);

                switch (message.Content.Instance.Type)
                {
                    case UserActionType.Joined:
                        return this.Users.TryJoin(user);
                    case UserActionType.Left:
                        return this.Users.TryLeave(user);
                    default:
                        throw new InvalidOperationException();
                }
            }

            return false;
        }

        public bool Process(in NetIncomingMessage<NetTargetActionMessage> message)
        {
            if (!_authorizer.Authorize(NetworkAuthorization.Slave, message))
            {

            }

            return false;
        }

        private void HandleUserJoined(RoomUserService sender, User newUser)
        {
            if(_authorizer.CurrentNetworkAuthorization != NetworkAuthorization.Master)
            {
                return;
            }

            // Broadcast the new user to all existing connections...
            this.Components.Get<NetMessenger>().Create<UserActionMessage>(UserActionMessage.Joined(newUser)).Enqueue();

            if (newUser.NetPeer is null)
            {
                return;
            }

            // Broadcast all existing users to the new connection...
            foreach (User user in this.Users)
            {
                if (user != newUser)
                {
                    this.Components.Get<NetMessenger>().Create<UserActionMessage>(UserActionMessage.Joined(user)).AddRecipient(newUser.NetPeer).Enqueue();
                }
            }
        }

        private void HandleUserLeft(RoomUserService sender, User oldUser)
        {
            if (_authorizer.CurrentNetworkAuthorization != NetworkAuthorization.Master)
            {
                return;
            }

            // Broadcast the old user to all existing connections...
            this.Components.Get<NetMessenger>().Create<UserActionMessage>(UserActionMessage.Left(oldUser)).Enqueue();
        }
    }
}
