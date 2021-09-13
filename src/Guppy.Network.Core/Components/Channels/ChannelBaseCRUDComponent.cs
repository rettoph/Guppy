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
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Entity.Messages.Add(Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity, Guppy.Network.Constants.MessageContexts.InternalReliableDefault);
            this.Entity.Messages.Add(Guppy.Network.Constants.Messages.Channel.UpdateNetworkEntity, Guppy.Network.Constants.MessageContexts.InternalReliableDefault);
            this.Entity.Messages.Add(Guppy.Network.Constants.Messages.Channel.PingNetworkEntity, Guppy.Network.Constants.MessageContexts.InternalUnreliableDefault);
            this.Entity.Messages.Add(Guppy.Network.Constants.Messages.Channel.DeleteNetworkEntity, Guppy.Network.Constants.MessageContexts.InternalReliableDefault);
        }

        protected override void Release()
        {
            base.Release();
        }
        #endregion
    }
}
