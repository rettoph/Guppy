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
using Guppy.Threading.Utilities;

namespace Guppy.Network.Components.Scenes
{
    internal class NetworkSceneBaseCRUDComponent : NetworkComponent<NetworkScene>
    {
        #region Protected Fields
        protected GuppyServiceProvider provider { get; private set; }
        protected NetworkEntityList networkEntities { get; private set; }
        protected ThreadQueue sceneThread { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.provider = provider;
            this.networkEntities = provider.GetService<NetworkEntityList>();
            this.sceneThread = provider.GetService<ThreadQueue>(Guppy.Constants.ServiceConfigurationKeys.SceneUpdateThreadQueue);

            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity].OnRead += this.ReadCreateNetworkEntityMessage;
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.PingNetworkEntity].OnRead += this.ReadPingNetworkEntityMessage;
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.DeleteNetworkEntity].OnRead += this.ReadDeleteNetworkEntityMessage;
        }

        protected override void Release()
        {
            base.Release();

            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity].OnRead -= this.ReadCreateNetworkEntityMessage;
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.PingNetworkEntity].OnRead -= this.ReadPingNetworkEntityMessage;
            this.Entity.Channel.Messages[Guppy.Network.Constants.Messages.Channel.DeleteNetworkEntity].OnRead -= this.ReadDeleteNetworkEntityMessage;

            this.provider = default;
        }
        #endregion

        #region Message Handlers
        protected virtual void ReadCreateNetworkEntityMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            this.sceneThread.Enqueue(_ =>
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
            });
        }

        private void ReadPingNetworkEntityMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            this.sceneThread.Enqueue(_ =>
            {
                Guid entityId = im.ReadGuid();
                INetworkEntity entity = this.networkEntities.GetById(entityId);

                entity?.Messages.Read(im);
            });
        }

        private void ReadDeleteNetworkEntityMessage(MessageTypeManager sender, NetIncomingMessage im)
        {
            this.sceneThread.Enqueue(_ =>
            {
                Guid entityId = im.ReadGuid();
                INetworkEntity entity = this.networkEntities.GetById(entityId);

                if (entity == default)
                    return;

                entity.Messages.Read(im);
                entity.TryRelease();
            });
        }
        #endregion
    }
}
