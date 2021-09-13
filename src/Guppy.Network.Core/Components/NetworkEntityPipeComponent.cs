using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Components
{
    internal sealed class NetworkEntityPipeComponent : RemoteHostComponent<INetworkEntity>
    {
        #region Private Fields
        private Boolean _setPipe;
        #endregion

        #region Lifecycle Metthods
        protected override void InitializeRemote(GuppyServiceProvider provider, NetworkAuthorization networkAuthorization)
        {
            base.InitializeRemote(provider, networkAuthorization);

            _setPipe = false;

            this.Entity.OnPipeChanged += this.HandleNetworkEntityPipeChanged;

            this.CleanNetworkEntityPipe(default, this.Entity.Pipe);
        }

        protected override void Release()
        {
            base.Release();

            this.Entity.OnPipeChanged -= this.HandleNetworkEntityPipeChanged;
        }
        #endregion

        #region Helper Methods
        private void CleanNetworkEntityPipe(IPipe old, IPipe value)
        {
            // Remove the entity from the old pipe and add it into the new pipe.
            old?.TryRemove(this.Entity);
            value?.TryAdd(this.Entity, old);

            _setPipe = true;
        }
        #endregion

        #region Event Handlers
        private void HandleNetworkEntityPipeChanged(INetworkEntity sender, IPipe old, IPipe value)
        {
            this.CleanNetworkEntityPipe(old, value);
        }
        #endregion
    }
}
