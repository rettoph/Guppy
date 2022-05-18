using Guppy.EntityComponent;
using Guppy.Network.Attributes;
using Guppy.Network.Entities;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Security.Providers;
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
    internal sealed class NetSystemMessengerRemoteSlaveComponent : Component<NetSystemMessenger>, ISubscriber<NetIncomingMessage<UserActionMessage>>
    {
        private readonly IUserProvider _users;

        public readonly NetScope Scope;
        public NetSystemMessenger Messenger { get; private set; }

        public NetSystemMessengerRemoteSlaveComponent(NetScope scope, IUserProvider users)
        {
            _users = users;
            this.Scope = scope;
            this.Messenger = default!;
        }

        protected override void Initialize(NetSystemMessenger entity)
        {
            this.Messenger = entity;

            this.Messenger.Messages.Subscribe(this);
        }

        protected override void Uninitilaize()
        {
            this.Messenger.Messages.Unsubscribe(this);
        }

        public bool Process(in NetIncomingMessage<UserActionMessage> message)
        {
            var user = _users.UpdateOrCreate(message.Content.Instance.Id, message.Content.Instance.Claims);

            switch (message.Content.Instance.Type)
            {
                case UserActionType.Joined:
                    return this.Scope.Users.TryJoin(user);
                case UserActionType.Left:
                    return this.Scope.Users.TryLeave(user);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
