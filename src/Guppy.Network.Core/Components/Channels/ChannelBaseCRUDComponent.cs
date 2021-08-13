using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Utilities;
using Guppy.Extensions.DependencyInjection;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;

namespace Guppy.Network.Components.Channels
{
    /// <summary>
    /// Primary component responsible for creating an IChannel's CRUD
    /// related message delegates.
    /// </summary>
    class ChannelBaseCRUDComponent : Component<IChannel>
    {
        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            this.Entity.OnStatus[ServiceStatus.Initializing] += this.HandleEntityInitializing;
        }

        protected override void Release()
        {
            base.Release();

            this.Entity.OnStatus[ServiceStatus.Initializing] -= this.HandleEntityInitializing;
        }
        #endregion

        #region
        private void HandleEntityInitializing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            this.Entity.Messages.Add(Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity, Guppy.Network.Constants.MessageContexts.InternalReliableDefault);
            this.Entity.Messages.Add(Guppy.Network.Constants.Messages.Channel.UpdateNetworkEntity, Guppy.Network.Constants.MessageContexts.InternalReliableDefault);
            this.Entity.Messages.Add(Guppy.Network.Constants.Messages.Channel.PingNetworkEntity, Guppy.Network.Constants.MessageContexts.InternalUnreliableDefault);
            this.Entity.Messages.Add(Guppy.Network.Constants.Messages.Channel.DeleteNetworkEntity, Guppy.Network.Constants.MessageContexts.InternalReliableDefault);
        }
        #endregion
    }
}
