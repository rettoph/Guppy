using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.Network.Contexts;
using Guppy.Network.Extensions;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Services;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Channels
{
    public abstract class Channel : Entity, IChannel
    {
        #region Private Fields
        private NetOutgoingMessageService _outgoingMessages;
        #endregion

        #region Public Properties
        public new Int16 Id { get; internal set; }
        public MessageManager Messages { get; private set; }
        #endregion

        #region IChannel Implementation
        Int16 IChannel.Id
        {
            get => this.Id;
            set => this.Id = value;
        }

        public PipeList Pipes { get; private set; }

        public UserList Users { get; private set; }
        #endregion

        #region Lifeycycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            this.Messages = new MessageManager();
            this.Messages.Signer = this.DefaultMessageSigner;
            this.Messages.DefaultFactory = this.DefaultMessageFactory;
        }

        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _outgoingMessages);

            this.Messages.Add(Constants.Messages.Channel.UserJoined, Constants.MessageContexts.InternalReliableDefault);
            this.Messages.Add(Constants.Messages.Channel.UserLeft, Constants.MessageContexts.InternalReliableDefault);

            this.Users = provider.GetService<UserList>(Constants.ServiceConfigurations.TransientUserList);
            this.Pipes = provider.GetService<PipeList>((pipes, p, c) =>
            {
                pipes.channel = this;
            });
        }


        protected override void PostRelease()
        {
            base.PostRelease();

            this.Messages.Clear();

            _outgoingMessages = null;

            this.Users.TryRelease();
            this.Pipes.TryRelease();

            this.Users = null;
            this.Pipes = null;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.Messages = null;
        }
        #endregion

        #region Frame Methods
        public virtual void TryUpdate()
        {
            this.Update();
        }

        protected virtual void Update()
        {
            // 
        }
        #endregion

        #region Helper Methods
        protected virtual void DefaultMessageSigner(NetOutgoingMessage om)
            => (this as IChannel).SignMessage(om);

        protected virtual NetOutgoingMessage DefaultMessageFactory(NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
            => _outgoingMessages.CreateMessage(context, recipients);
        #endregion
    }
}
