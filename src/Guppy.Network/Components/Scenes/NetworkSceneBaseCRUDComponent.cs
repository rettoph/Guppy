using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network.Utilities;
using Lidgren.Network;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Interfaces;

namespace Guppy.Network.Components.Scenes
{
    internal class NetworkSceneBaseCRUDComponent : RemoteHostComponent<NetworkScene>
    {
        #region Protected Fields
        protected ServiceProvider provider { get; private set; }
        protected NetworkEntityList networkEntities { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.provider = provider;
            this.networkEntities = provider.GetService<NetworkEntityList>();
        }

        protected override void Release()
        {
            base.Release();

            this.provider = default;
        }

        protected override void HandleEntityInitializing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            base.HandleEntityInitializing(sender, old, value);

            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity].OnRead += this.ReadCreateNetworkEntityMessage;
        }

        protected override void HandleEntityReleasing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            base.HandleEntityReleasing(sender, old, value);

            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity].OnRead -= this.ReadCreateNetworkEntityMessage;
        }
        #endregion

        #region Message Handlers
        protected virtual void ReadCreateNetworkEntityMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            var keyId = im.ReadUInt32();
            var key = this.provider.ServiceConfigurationKeys[keyId];
            var serviceId = im.ReadGuid();
            var service = this.networkEntities.Create<INetworkEntity>(
                key, 
                (networkEntity, p, c) => 
                {
                    networkEntity.Messages.Read(im);
                }, 
                serviceId);
            
        }
        #endregion
    }
}
