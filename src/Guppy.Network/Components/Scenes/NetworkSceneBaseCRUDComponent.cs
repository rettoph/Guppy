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
        protected GuppyServiceProvider provider { get; private set; }
        protected NetworkEntityList networkEntities { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            this.provider = provider;
            this.networkEntities = provider.GetService<NetworkEntityList>();

            this.Entity.OnStatus[ServiceStatus.Initializing] += this.HandleEntityInitializing;
            this.Entity.OnStatus[ServiceStatus.Releasing] += this.HandleEntityReleasing;
        }

        protected override void Release()
        {
            base.Release();

            this.Entity.OnStatus[ServiceStatus.Initializing] -= this.HandleEntityInitializing;
            this.Entity.OnStatus[ServiceStatus.Releasing] -= this.HandleEntityReleasing;

            this.provider = default;
        }

        private void HandleEntityInitializing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity].OnRead += this.ReadCreateNetworkEntityMessage;
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.PingNetworkEntity].OnRead += this.ReadPingNetworkEntityMessage;
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.DeleteNetworkEntity].OnRead += this.ReadDeleteNetworkEntityMessage;
        }

        private void HandleEntityReleasing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity].OnRead -= this.ReadCreateNetworkEntityMessage;
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.PingNetworkEntity].OnRead -= this.ReadPingNetworkEntityMessage;
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.DeleteNetworkEntity].OnRead -= this.ReadDeleteNetworkEntityMessage;
        }
        #endregion

        #region Message Handlers
        protected virtual void ReadCreateNetworkEntityMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            UInt32 keyId = im.ReadUInt32();
            ServiceConfigurationKey key = this.provider.ServiceConfigurationKeys[keyId];
            Guid entityId = im.ReadGuid();
            INetworkEntity entity = this.networkEntities.GetById(entityId) ?? this.networkEntities.Create<INetworkEntity>(
                key, 
                (networkEntity, _, _) => 
                {
                    networkEntity.Messages.Read(im);
                }, 
                entityId);
        }

        private void ReadPingNetworkEntityMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            Guid entityId = im.ReadGuid();
            INetworkEntity entity = this.networkEntities.GetById(entityId);

            entity?.Messages.Read(im);
        }

        private void ReadDeleteNetworkEntityMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            Guid entityId = im.ReadGuid();
            INetworkEntity entity = this.networkEntities.GetById(entityId);

            if (entity == default)
                return;

            entity.Messages.Read(im);
            entity.TryRelease();
        }
        #endregion
    }
}
