﻿using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.log4net;
using Guppy.Extensions.System.Collections;
using Guppy.Network.Channels;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Security;
using Guppy.Network.Services;
using Guppy.Utilities;
using Lidgren.Network;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Service, IPeer
    {
        #region Private Fields
        private ServiceProvider _provider;
        private ILog _log;
        private NetOutgoingMessageService _outgoing;
        private NetPeer _peer;
        private NetIncomingMessage _im;        
        #endregion

        #region Public Properties
        /// <summary>
        /// By default all <see cref="Peer"/> id's are set to <see cref="Int16.MinValue"/>.
        /// </summary>
        public new Int16 Id => Int16.MinValue;
        #endregion

        #region Protected Properties
        /// <summary>
        /// The <see cref="ServiceConfigurationDescriptor.Key"/> to be used when creating a new
        /// <see cref="IChannel"/> instance.
        /// </summary>
        protected abstract ServiceConfigurationKey channelServiceConfigurationkey { get; }
        #endregion

        #region IPeer Implementation
        /// <inheritdoc />
        public UserList Users { get; private set; }

        /// <inheritdoc />
        public ChannelList Channels { get; private set; }

        /// <inheritdoc />
        public IUser CurrentUser { get; internal set; }

        /// <inheritdoc />
        public Dictionary<NetIncomingMessageType, OnEventDelegate<IPeer, NetIncomingMessage>> OnIncomingMessageTypeRecieved { get; private set; }

        /// <inheritdoc />
        public event OnEventDelegate<IPeer, NetIncomingMessage> OnIncomingMessageRecieved;

        /// <inheritdoc />
        ServiceConfigurationKey IPeer.ChannelServiceConfigurationKey => this.channelServiceConfigurationkey;
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.OnIncomingMessageTypeRecieved = DictionaryHelper.BuildEnumDictionary<NetIncomingMessageType, OnEventDelegate<IPeer, NetIncomingMessage>>();

            this.OnIncomingMessageRecieved += this.HandleIncomingMessageRecieved;
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Users = provider.GetService<UserList>();
            this.Channels = provider.GetService<ChannelList>((channels, p, c) =>
            {
                channels.channelServiceConfigurationKey = this.channelServiceConfigurationkey;
            });
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _provider = provider;

            provider.Service(out _peer);
            provider.Service(out _log);
            provider.Service(out _outgoing);

            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.Data] += this.HandleDataMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.DebugMessage] += this.HandleDebugMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.VerboseDebugMessage] += this.HandleVerboseDebugMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.ErrorMessage] += this.HandleErrorMessage;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Users.TryRelease();
            this.Channels.TryRelease();

            _peer = null;
            _log = null;
            _outgoing = null;
            this.Users = null;
            this.Channels = null;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnIncomingMessageTypeRecieved.Clear();

            this.OnIncomingMessageTypeRecieved = null;

            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.Data] -= this.HandleDataMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.DebugMessage] -= this.HandleDebugMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.VerboseDebugMessage] -= this.HandleVerboseDebugMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.ErrorMessage] -= this.HandleErrorMessage;
        }
        #endregion

        #region Helper Methods
        /// <inheritdoc />
        public void Start()
        {
            _peer.Start();
        }

        /// <inheritdoc />
        public IUser CreateUser(params Claim[] claims)
            => _provider.GetService<IUser>((user, p, c) =>
            {
                claims.ForEach(claim => user.AddClaim(claim));
            });
        #endregion

        #region Frame Methods
        public virtual void TryUpdate()
        {
            this.Update();
        }

        protected virtual void Update()
        {
            while ((_im = _peer.ReadMessage()) != default)
                this.OnIncomingMessageRecieved.Invoke(this, _im);

            _outgoing.Flush();
        }
        #endregion

        #region MessageRecieved Handlers
        private void HandleIncomingMessageRecieved(IPeer sender, NetIncomingMessage im)
        {
            this.OnIncomingMessageTypeRecieved[_im.MessageType]?.Invoke(this, _im);
        }

        private void HandleDataMessage(IPeer sender, NetIncomingMessage im)
        {
            // There is a global agreement that all custom messages are handled by a recieving
            // channel, as signed by their prefix value.
            var channelId = im.ReadInt16();
            this.Channels.GetById(channelId).Messages.Read(im);
        }

        private void HandleDebugMessage(IPeer sender, NetIncomingMessage im)
        {
            _log.Debug($"{im.ReadString()}");
        }

        private void HandleVerboseDebugMessage(IPeer sender, NetIncomingMessage im)
        {
            _log.Verbose($"{im.ReadString()}");
        }

        private void HandleErrorMessage(IPeer sender, NetIncomingMessage im)
        {
            _log.Error($"{im.ReadString()}");
        }
        #endregion
    }
}
