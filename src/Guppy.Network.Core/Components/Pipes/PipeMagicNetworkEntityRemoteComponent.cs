using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using Guppy.EntityComponent.Interfaces;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.EventArgs;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
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
    internal sealed class PipeMagicNetworkEntityRemoteComponent : Component<Pipe>
    {
        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Entity.OnNetworkEntityAdded += this.HandleMasterNetworkEntityAdded;
            this.Entity.Users.OnUserAdded += this.HandleMasterUserAdded;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Entity.OnNetworkEntityAdded -= this.HandleMasterNetworkEntityAdded;
            this.Entity.Users.OnUserAdded -= this.HandleMasterUserAdded;
        }
        #endregion

        #region Event Handlers
        private void HandleMasterUserAdded(UserList sender, UserEventArgs args)
        {
            if(args.User.NetPeer is not null)
            {
                // Broadcast a create message for every entity within the pipe...
                foreach (IMagicNetworkEntity entity in this.Entity.NetworkEntities)
                {
                    entity.SendMessage(new CreateNetworkEntityMessage()
                    {
                        ServiceConfigurationId = entity.ServiceConfiguration.Id
                    }, args.User.NetPeer);
                }
            }
        }

        private void HandleMasterNetworkEntityAdded(Pipe sender, MagicNetworkEntityPipeEventArgs args)
        {
            if(args.OldPipe is null)
            { // This is the first pipe the entity has been put into...
                if(args.Entity.Status == ServiceStatus.Ready)
                {
                    // Broadcast a create message to all users.
                    args.Entity.SendMessage(new CreateNetworkEntityMessage()
                    {
                        ServiceConfigurationId = args.Entity.ServiceConfiguration.Id
                    });
                }
                else
                {
                    // Wait for the entity to be ready...
                    args.Entity.OnStatusChanged += this.HandleNetworkEntityWithFirstPipeReady;
                }
            }
        }

        private void HandleNetworkEntityWithFirstPipeReady(IService sender, ServiceStatus old, ServiceStatus value)
        {
            if(value == ServiceStatus.Ready && sender is IMagicNetworkEntity entity)
            {
                entity.OnStatusChanged -= this.HandleNetworkEntityWithFirstPipeReady;
                entity.SendMessage(new CreateNetworkEntityMessage()
                {
                    ServiceConfigurationId = entity.ServiceConfiguration.Id
                });
            }
        }
        #endregion
    }
}
