using Guppy.DependencyInjection;
using Guppy.Lists;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Lists
{
    public class NetworkEntityList : FactoryServiceList<INetworkEntity>
    {
        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnAdded += this.HandleNetworkEntityAdded;
            this.OnRemoved += this.HandleNetworkEntityRemoved;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.OnAdded -= this.HandleNetworkEntityAdded;
            this.OnRemoved -= this.HandleNetworkEntityRemoved;
        }
        #endregion

        #region Event Handlers
        private void HandleNetworkEntityAdded(IServiceList<INetworkEntity> sender, INetworkEntity item)
        {
            // Automatically attempt to add the item to its pipe...
            item.Pipe?.NetworkEntities.TryAdd(item);

            // Begin tracking all internal pipe updates...
            item.OnPipeChanged += this.HandleNetworkEntityPipeChanged;
        }

        private void HandleNetworkEntityRemoved(IServiceList<INetworkEntity> sender, INetworkEntity item)
        {
            // Undo everything setup within the Added handler
            item.Pipe?.NetworkEntities.TryRemove(item);
            item.OnPipeChanged -= this.HandleNetworkEntityPipeChanged;
        }

        private void HandleNetworkEntityPipeChanged(INetworkEntity item, IPipe old, IPipe value)
        {
            // Remove the network entity from its old pipe and add it into its new pipe.
            old?.NetworkEntities.TryRemove(item);
            value?.NetworkEntities.TryAdd(item);
        }
        #endregion
    }
}
