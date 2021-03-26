using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Network.Contexts;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Interfaces;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Guppy.Network
{
    public abstract class NetworkEntity : Entity, INetworkService
    {
        #region Private Fields
        private IPipe _pipe;
        #endregion

        #region Public Properties
        /// <summary>
        /// <para>The primary entity owning pipe. When a user joins the pipe they will
        /// should automatically recieve relevant data about it.</para>
        /// </summary>
        public IPipe Pipe
        {
            get => _pipe;
            set => this.OnPipeChanged.InvokeIf(_pipe != value, this, ref _pipe, value);
        }
        #endregion

        #region Events
        public event OnChangedEventDelegate<NetworkEntity, IPipe> OnPipeChanged;
        #endregion

        #region INetworkService Implementation
        public MessageManager Messages { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.Messages = new MessageManager(this.SignMessage, this.DefaultMessageFactory);

            this.Messages.Add(GuppyNetworkConstants.Messages.NetworEntity.Create, GuppyNetworkCoreConstants.MessageContexts.InternalReliableDefault, this.CreateMessageFactory);
            this.Messages.Add(GuppyNetworkConstants.Messages.NetworEntity.Update, GuppyNetworkCoreConstants.MessageContexts.InternalReliableSecondary);
            this.Messages.Add(GuppyNetworkConstants.Messages.NetworEntity.Remove, GuppyNetworkCoreConstants.MessageContexts.InternalReliableDefault, this.RemoveMessageFactory);
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.Messages.Remove(GuppyNetworkConstants.Messages.NetworEntity.Create);
            this.Messages.Remove(GuppyNetworkConstants.Messages.NetworEntity.Update);
            this.Messages.Remove(GuppyNetworkConstants.Messages.NetworEntity.Remove);

            this.Messages = null;
        }
        #endregion

        #region Helper Methods
        protected virtual void SignMessage(NetOutgoingMessage om)
        {
            om.Write(this.Id);
        }

        /// <summary>
        /// Helper method for the internal creation of messages.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        protected NetOutgoingMessage DefaultMessageFactory(NetOutgoingMessageContext context, NetConnection recipient, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter)
            => this.Pipe.Messages[GuppyNetworkConstants.Messages.NetworkScene.UpdateEntity].Create(context, recipient, filter);

        private NetOutgoingMessage CreateMessageFactory(NetOutgoingMessageContext context, NetConnection recipient, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter)
            => this.Pipe.Messages[GuppyNetworkConstants.Messages.NetworkScene.CreateEntity].Create(context, recipient, filter);

        private NetOutgoingMessage RemoveMessageFactory(NetOutgoingMessageContext context, NetConnection recipient, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter)
            => this.Pipe.Messages[GuppyNetworkConstants.Messages.NetworkScene.RemoveEntity].Create(context, recipient, filter);
        #endregion
    }
}
