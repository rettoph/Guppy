using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Components.Lists
{
    /// <summary>
    /// Simple class used to automatically add <see cref="ILayerable"/> instance
    /// of <see cref="INetworkEntity"/> implementations into the <see cref="LayerList"/>
    /// </summary>
    internal sealed class NetworkServiceListLayerableComponent : Component<NetworkEntityList>
    {
        #region Private Fields
        private LayerableList _layerables;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _layerables);

            this.Entity.OnCreated += this.HandleItemCreated;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Entity.OnCreated -= this.HandleItemCreated;
        }
        #endregion

        #region Event Handlers
        private void HandleItemCreated(INetworkEntity item)
        {
            if (item is ILayerable layerable)
                _layerables.TryAdd(layerable);
        }
        #endregion
    }
}
