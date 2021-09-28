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
    internal sealed class PipeMasterCRUDComponent : NetworkComponent<IPipe>
    {
        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            this.Entity.Users.OnAdded += this.HandleUserAddedToPipe;
            this.Entity.OnNetworkEnityAddedToPipe += this.HandleNetworkEnityAddedToPipe;
        }

        protected override void Release()
        {
            base.Release();

            this.Entity.Users.OnAdded -= this.HandleUserAddedToPipe;
            this.Entity.OnNetworkEnityAddedToPipe -= this.HandleNetworkEnityAddedToPipe;
        }
        #endregion

        #region Event Handlers
        private void HandleUserAddedToPipe(IServiceList<IUser> sender, IUser args)
        {
            // Broadcast every single networkEntity within the pipe to the new user.
            foreach(INetworkEntity networkEntity in this.Entity.NetworkEntities)
                networkEntity.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Create].Create(args.Connection.Yield());
        }

        private void HandleNetworkEnityAddedToPipe(IPipe sender, INetworkEntity networkEntity, IPipe oldPipe)
        {
            if (oldPipe != default)
            { // If the entity was in another pipe...
                // Broadcast delete message through the old pipe to users who are not also in the new pipe...
                networkEntity.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Delete].Create(
                    oldPipe.Users.Connections.Except(this.Entity.Users.Connections));

                // Broadcast create message through the pipe to users who were not also in the old pipe...
                networkEntity.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Create].Create(
                    this.Entity.Users.Connections.Except(oldPipe.Users.Connections));
            }
            else
            { // If this is the first pipe the entity has  been put in...
                // Broadcast create message through the pipe to all users
                networkEntity.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Create].Create(this.Entity);
            }

        }
        #endregion
    }
}
