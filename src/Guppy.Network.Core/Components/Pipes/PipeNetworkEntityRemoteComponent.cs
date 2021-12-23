using Guppy.EntityComponent.DependencyInjection;
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
        protected override void InitializeRemote(ServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            base.InitializeRemote(provider, networkAuthorization);

            if(networkAuthorization == NetworkAuthorization.Master)
            {
                this.Entity.OnNetworkEntityAdded += this.HandleMasterNetworkEntityAdded;
                this.Entity.Users.OnUserAdded += this.HandleMasterUserAdded;
            }
        }

        protected override void ReleaseRemote(NetworkAuthorization networkAuthorization)
        {
            base.ReleaseRemote(networkAuthorization);

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
                    entity.SendMessage<CreateNetworkEntityMessage>(args.User.NetPeer);
                }
            }
        }

        private void HandleMasterNetworkEntityAdded(Pipe sender, NetworkEntityPipeEventArgs args)
        {
            if(args.OldPipe is null)
            { // This is the first pipe the entity has been put into...
                // Broadcast a create message to all users.
                args.Entity.SendMessage<CreateNetworkEntityMessage>();
            }
        }
        #endregion
    }
}
