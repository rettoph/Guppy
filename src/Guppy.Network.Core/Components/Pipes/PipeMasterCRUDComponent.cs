using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Guppy.Lists.Interfaces;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Components.Scenes
{
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    internal sealed class PipeMasterCRUDComponent : RemoteHostComponent<IPipe>
    {
        #region Lifecycle Methods
        protected override void HandleEntityInitializing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            base.HandleEntityInitializing(sender, old, value);

            this.Entity.Users.OnAdded += this.HandleUserAddedToPipe;
            this.Entity.NetworkEntities.OnAdded += this.HandleNetworkEntityAddedToPipe;
            this.Entity.NetworkEntities.OnRemoved += this.HandleNetworkEntityRemovedFromPipe;
        }

        protected override void HandleEntityReleasing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            base.HandleEntityReleasing(sender, old, value);

            this.Entity.Users.OnAdded -= this.HandleUserAddedToPipe;
            this.Entity.NetworkEntities.OnAdded -= this.HandleNetworkEntityAddedToPipe;
            this.Entity.NetworkEntities.OnRemoved -= this.HandleNetworkEntityRemovedFromPipe;
        }
        #endregion

        #region Event Handlers
        private void HandleUserAddedToPipe(IServiceList<IUser> sender, IUser args)
        {
            // Broadcast every single networkEntity within the pipe to the new user.
            foreach(INetworkEntity networkEntity in this.Entity.NetworkEntities)
                networkEntity.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Create].Create(args.Connection.Yield());
        }

        private void HandleNetworkEntityAddedToPipe(IServiceList<INetworkEntity> sender, INetworkEntity networkEntity)
        {
            // Broadcast create message through the pipe...
            networkEntity.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Create].Create(this.Entity);
        }

        private void HandleNetworkEntityRemovedFromPipe(IServiceList<INetworkEntity> sender, INetworkEntity networkEntity)
        {
            // Broadcast the delete message to all users who are not also in the new NE's pipe.
            networkEntity.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Delete].Create(
                this.Entity.Users.Connections.Except(networkEntity.Pipe?.Users.Connections ?? Enumerable.Empty<NetConnection>()));
        }
        #endregion
    }
}
