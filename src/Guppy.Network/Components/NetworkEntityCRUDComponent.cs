using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.System;
using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using Guppy.Network.Scenes;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Components
{
    internal sealed class NetworkEntityCRUDComponent : RemoteHostComponent<INetworkEntity>
    {
        #region Private Fields
        private NetworkScene _scene;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _scene);

            this.Entity.Messages.DefaultFactory = this.PingMessageFactory;

            this.Entity.Messages.Add(
                messageType: Guppy.Network.Constants.Messages.NetworkEntity.Create, 
                defaultContext: Guppy.Network.Constants.MessageContexts.InternalReliableDefault,
                factory: this.CreateMessageFactory);

            this.Entity.Messages.Add(
                messageType: Guppy.Network.Constants.Messages.NetworkEntity.Update,
                defaultContext: Guppy.Network.Constants.MessageContexts.InternalReliableDefault,
                factory: this.UpdateMessageFactory);

            this.Entity.Messages.Add(
                messageType: Guppy.Network.Constants.Messages.NetworkEntity.Ping,
                defaultContext: Guppy.Network.Constants.MessageContexts.InternalUnreliableDefault,
                factory: this.PingMessageFactory);

            this.Entity.Messages.Add(
                messageType: Guppy.Network.Constants.Messages.NetworkEntity.Delete,
                defaultContext: Guppy.Network.Constants.MessageContexts.InternalReliableDefault,
                factory: this.DeleteMessageFactory);
        }

        protected override void Release()
        {
            base.Release();

            this.Entity.Messages.Remove(Guppy.Network.Constants.Messages.NetworkEntity.Create);
            this.Entity.Messages.Remove(Guppy.Network.Constants.Messages.NetworkEntity.Update);
            this.Entity.Messages.Remove(Guppy.Network.Constants.Messages.NetworkEntity.Ping);
            this.Entity.Messages.Remove(Guppy.Network.Constants.Messages.NetworkEntity.Delete);
        }
        #endregion

        #region Message Factories
        private NetOutgoingMessage CreateMessageFactory(NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
            => _scene.Channel.Messages[Guppy.Network.Constants.Messages.Channel.CreateNetworkEntity].Create(context, recipients).Then(om =>
            {
                om.Write(this.Entity.ServiceConfiguration.Key.Id);
            });
        
        private NetOutgoingMessage UpdateMessageFactory(NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
            => _scene.Channel.Messages[Guppy.Network.Constants.Messages.Channel.UpdateNetworkEntity].Create(context, recipients);
        
        private NetOutgoingMessage PingMessageFactory(NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
            => _scene.Channel.Messages[Guppy.Network.Constants.Messages.Channel.PingNetworkEntity].Create(context, recipients);
        
        private NetOutgoingMessage DeleteMessageFactory(NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
            => _scene.Channel.Messages[Guppy.Network.Constants.Messages.Channel.DeleteNetworkEntity].Create(context, recipients);
        #endregion
    }
}
