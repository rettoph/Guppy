using Guppy.DependencyInjection;
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
    public abstract class Channel : NetworkService, IChannel
    {
        #region Private Fields
        private NetOutgoingMessageService _outgoingMessages;
        #endregion

        #region Public Properties
        public new Int16 Id { get; internal set; }
        #endregion

        #region IPipe Implementation
        IChannel IPipe.Channel
        {
            get => this;
            set => throw new NotImplementedException();
        }

        public UserList Users { get; private set; }
        #endregion

        #region IChannel Implementation
        Int16 IChannel.Id
        {
            get => this.Id;
            set => this.Id = value;
        }

        public PipeList Pipes { get; private set; }
        #endregion

        #region Lifeycycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.Messages.Add(GuppyNetworkCoreConstants.Messages.Channel.UserJoined, GuppyNetworkCoreConstants.MessageContexts.InternalReliableDefault);
            this.Messages.Add(GuppyNetworkCoreConstants.Messages.Channel.UserLeft  , GuppyNetworkCoreConstants.MessageContexts.InternalReliableDefault);
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _outgoingMessages);

            this.Users = provider.GetService<UserList>(GuppyNetworkCoreConstants.ServiceConfigurations.TransientUserList);
            this.Pipes = provider.GetService<PipeList>();
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _outgoingMessages = null;

            this.Users.TryRelease();
            this.Pipes.TryRelease();

            this.Users = null;
            this.Pipes = null;
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
        protected override void SignMessage(NetOutgoingMessage om)
            => (this as IChannel).SignMessage(om);

        protected override NetOutgoingMessage DefaultMessageFactory(NetOutgoingMessageContext context, NetConnection recipient, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter)
            => this.CreateMessage(context, recipient, filter);

        /// <inheritdoc />
        public NetOutgoingMessage CreateMessage(NetOutgoingMessageContext context, NetConnection recipient = null, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter = null)
            => _outgoingMessages.CreateMessage(this, context, recipient, filter);
        #endregion
    }
}
