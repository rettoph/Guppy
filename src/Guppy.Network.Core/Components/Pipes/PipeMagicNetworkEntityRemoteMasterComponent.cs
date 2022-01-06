using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using Guppy.EntityComponent.Interfaces;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.EventArgs;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.EventArgs;
using Guppy.Network.Security.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components.Pipes
{
    /// <summary>
    /// This component will automatically transmit entity data back and fourth
    /// between peers
    /// </summary>
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    internal sealed class PipeMagicNetworkEntityRemoteMasterComponent : Component<Pipe>
    {
        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Entity.OnNetworkEntityAdded += this.HandleNetworkEntityAdded;
            this.Entity.OnNetworkEntityRemoved += this.HandleNetworkEntityRemoved;
            this.Entity.Users.OnEvent += this.HandleUserListEvent;
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            this.Entity.OnNetworkEntityAdded -= this.HandleNetworkEntityAdded;
            this.Entity.OnNetworkEntityRemoved -= this.HandleNetworkEntityRemoved;
            this.Entity.Users.OnEvent -= this.HandleUserListEvent;
        }
        #endregion

        #region Event Handlers
        private void HandleUserListEvent(UserList sender, UserListEventArgs args)
        {
            switch (args.Action)
            {
                case UserListAction.Added:
                    this.HandleUserAdded(args.User);
                    break;
                case UserListAction.Removed:
                    this.HandleUserRemoved(args.User);
                    break;
            }
        }

        private void HandleUserAdded(User user)
        {
            if(user.NetPeer is not null)
            {
                // Broadcast a create message for every entity within the pipe...
                foreach (IMagicNetworkEntity entity in this.Entity.NetworkEntities)
                {
                    entity.SendMessage(new CreateNetworkEntityMessage()
                    {
                        ServiceConfigurationId = entity.ServiceConfiguration.Id,
                    }, user.NetPeer);
                }
            }
        }

        private void HandleUserRemoved(User user)
        {
            if (user.NetPeer is not null)
            {
                // Broadcast a create message for every entity within the pipe...
                foreach (IMagicNetworkEntity entity in this.Entity.NetworkEntities)
                {
                    entity.SendMessage<DisposeNetworkEntityMessage>(user.NetPeer);
                }
            }
        }

        private void HandleNetworkEntityAdded(Pipe sender, MagicNetworkEntityPipeEventArgs args)
        {
            if(args.OldPipe is null)
            { // This is the first pipe the entity has been put into...
                args.Entity.SendMessage(new CreateNetworkEntityMessage()
                {
                    ServiceConfigurationId = args.Entity.ServiceConfiguration.Id,
                });
            }
            else
            { // The entity just changed pipes...
                // throw new NotImplementedException();
            }
        }

        private void HandleNetworkEntityRemoved(Pipe sender, MagicNetworkEntityPipeEventArgs args)
        {
            if(args.NewPipe is null)
            { // The entity was NOT added into another pipe, so we can just remove it everywhere.
                args.Entity.SendMessage<DisposeNetworkEntityMessage>(this.Entity);
            }
        }
        #endregion
    }
}
