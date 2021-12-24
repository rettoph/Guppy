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
    internal sealed class PipeNetworkEntityRemoteComponent : NetworkComponent<Pipe>
    {
        #region Lifecycle Methods
        protected override void PreInitializeRemote(ServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            base.PreInitializeRemote(provider, networkAuthorization);

            if(networkAuthorization == NetworkAuthorization.Master)
            {
                this.Entity.OnNetworkEntityAdded += this.HandleMasterNetworkEntityAdded;
                this.Entity.Users.OnUserAdded += this.HandleMasterUserAdded;
            }
        }

        protected override void PostReleaseRemote(NetworkAuthorization networkAuthorization)
        {
            base.PostReleaseRemote(networkAuthorization);

            if (networkAuthorization == NetworkAuthorization.Master)
            {
                this.Entity.OnNetworkEntityAdded -= this.HandleMasterNetworkEntityAdded;
                this.Entity.Users.OnUserAdded -= this.HandleMasterUserAdded;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleMasterUserAdded(UserList sender, UserEventArgs args)
        {
            if(args.User.NetPeer is not null)
            {
                // Broadcast a create message for every entity within the pipe...
                foreach (INetworkEntity entity in this.Entity.NetworkEntities)
                {
                    entity.SendMessage<CreateNetworkEntityMessage>(args.User.NetPeer, message => message.ServiceConfigurationId = entity.ServiceConfiguration.Id);
                }
            }
        }

        private void HandleMasterNetworkEntityAdded(Pipe sender, NetworkEntityPipeEventArgs args)
        {
            if(args.OldPipe is null)
            { // This is the first pipe the entity has been put into...
                if(args.Entity.Status == ServiceStatus.Ready)
                {
                    // Broadcast a create message to all users.
                    args.Entity.SendMessage<CreateNetworkEntityMessage>(message => message.ServiceConfigurationId = args.Entity.ServiceConfiguration.Id);
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
            if(value == ServiceStatus.Ready && sender is INetworkEntity entity)
            {
                entity.OnStatusChanged -= this.HandleNetworkEntityWithFirstPipeReady;
                entity.SendMessage<CreateNetworkEntityMessage>(message => message.ServiceConfigurationId = entity.ServiceConfiguration.Id);
            }
        }
        #endregion
    }
}
