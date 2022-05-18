using Guppy.EntityComponent;
using Guppy.Network.Attributes;
using Guppy.Network.Entities;
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
    internal sealed class NetSystemMessengerRemoteMasterComponent : Component<NetSystemMessenger>
    {
        public readonly NetScope Scope;
        public NetSystemMessenger Messenger { get; private set; }

        public NetSystemMessengerRemoteMasterComponent(NetScope scope)
        {
            this.Scope = scope;
            this.Messenger = default!;
        }

        protected override void Initialize(NetSystemMessenger entity)
        {
            this.Messenger = entity;

            this.Scope.Users.OnUserJoined += this.HandleUserJoined;
            this.Scope.Users.OnUserLeft += this.HandleUserLeft;
        }

        protected override void Uninitilaize()
        {
            this.Scope.Users.OnUserJoined -= this.HandleUserJoined;
            this.Scope.Users.OnUserLeft -= this.HandleUserLeft;
        }

        private void HandleUserLeft(NetScopeUserService sender, User oldUser)
        {
            // Broadcast the old user to all existing connections...
            this.Messenger.Messages.CreateOutgoing<UserActionMessage>(UserActionMessage.Left(oldUser)).EnqueueSend();
        }

        private void HandleUserJoined(NetScopeUserService sender, User newUser)
        {
            // Broadcast the new user to all existing connections...
            this.Messenger.Messages.CreateOutgoing<UserActionMessage>(UserActionMessage.Joined(newUser)).EnqueueSend();
            
            
            if (newUser.NetPeer is null)
            {
                return;
            }
            
            // Broadcast all existing users to the new connection...
            foreach (User user in this.Scope.Users)
            {
                if(user != newUser)
                {
                    this.Messenger.Messages.CreateOutgoing<UserActionMessage>(UserActionMessage.Joined(user)).AddRecipient(newUser.NetPeer).EnqueueSend();
                }
            }
        }
    }
}
