using Guppy.DependencyInjection;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Guppy.Network.Drivers.Entities
{
    internal sealed class NetworkEntityMasterNetworkAuthorizationDiver : MasterNetworkAuthorizationDriver<NetworkEntity>
    {
        #region Lifecycle Methods
        protected override void InitializeRemote(NetworkEntity driven, ServiceProvider provider)
        {
            base.InitializeRemote(driven, provider);

            this.driven.OnPipeChanged += this.HandlePipeChanged;
        }

        protected override void ReleaseRemote(NetworkEntity driven)
        {
            base.ReleaseRemote(driven);

            this.CleanPipe(this.driven.Pipe, null);

            this.driven.OnPipeChanged -= this.HandlePipeChanged;
        }
        #endregion

        #region Helper Methods
        private void CleanPipe(IPipe old, IPipe value)
        {
            if(old != default)
            { // Unbind from old pipe...
                old.Users.OnAdded -= this.HandleUserAddedToPipe;
                old.Users.OnRemoved -= this.HandleUserRemovedFromPipe;
            }

            if(value != default)
            { // Bind to new pipe...
                value.Users.OnAdded += this.HandleUserAddedToPipe;
                value.Users.OnRemoved += this.HandleUserRemovedFromPipe;
            }
        }

        /// <summary>
        /// Broadcast some create & remove messages to any 
        /// missing user connections.
        /// </summary>
        /// <param name="old"></param>
        /// <param name="pipe"></param>
        private void BroadcastPipe(IPipe old, IPipe pipe)
        {
            this.driven.Messages[GuppyNetworkConstants.Messages.NetworEntity.Create].Create(filter: connections => connections.Except(old.Users.Connections));
            this.driven.Messages[GuppyNetworkConstants.Messages.NetworEntity.Remove].Create(filter: connections => old.Users.Connections.Except(connections));
        }
        #endregion

        #region Event Handlers
        private void HandlePipeChanged(NetworkEntity sender, IPipe old, IPipe value)
        {
            this.CleanPipe(old, value);
            this.BroadcastPipe(old, value);
        }

        private void HandleUserAddedToPipe(IServiceList<IUser> sender, IUser user)
        { // Alert the new user of the current entity...
            if (user.Connection.Status == NetConnectionStatus.Connected)
            {
                this.driven.Messages[GuppyNetworkConstants.Messages.NetworEntity.Create].Create(user.Connection);
            }
        }

        private void HandleUserRemovedFromPipe(IServiceList<IUser> sender, IUser user)
        { // Remove the users reference to the current entity...
            if(user.Connection.Status == NetConnectionStatus.Connected)
            {
                this.driven.Messages[GuppyNetworkConstants.Messages.NetworEntity.Remove].Create(user.Connection);
            }
        }
        #endregion
    }
}
