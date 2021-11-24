using Guppy.DependencyInjection;
using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Scenes;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network
{
    public class NetworkLayerable : Layerable, INetworkEntity
    {
        #region Private Fields
        private IPipe _pipe;
        #endregion

        #region INetworkService Implementation
        public MessageManager Messages { get; private set; }

        public virtual IPipe Pipe
        {
            get => _pipe;
            protected set => this.OnPipeChanged.InvokeIf(value != _pipe, this, ref _pipe, value);
        }

        IPipe INetworkEntity.Pipe
        {
            get => this.Pipe;
            set => this.Pipe = value;
        }
        #endregion

        #region Events
        public virtual event OnChangedEventDelegate<INetworkEntity, IPipe> OnPipeChanged;
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            this.Messages = new MessageManager();
            this.Messages.Signer = this.DefaultMessageSigner;
            this.Messages.DefaultFactory = this.DefaultMessageFactory;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Messages.Clear();
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.Messages = null;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Custom pre-appender for all internally created messages.
        /// </summary>
        /// <param name="om"></param>
        protected virtual void DefaultMessageSigner(NetOutgoingMessage om)
        {
            om.Write(this.Id);
        }

        /// <summary>
        /// Defines a default implementation for all messages created through the
        /// <see cref="INetworkService"/> instance's <see cref="Messages"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="context"></param>
        /// <param name="recipients"></param>
        /// <returns></returns>
        protected virtual void DefaultMessageFactory(Action<NetOutgoingMessage> writer, NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
