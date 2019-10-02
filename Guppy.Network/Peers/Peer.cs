﻿using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Configurations;
using Guppy.Pooling.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Collections;
using Guppy.Network.Utilitites.Delegaters;
using Microsoft.Xna.Framework;
using Guppy.Network.Security.Collections;
using Guppy.Network.Groups;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Concurrent;
using Guppy.Extensions.Concurrent;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Frameable
    {
        #region Private Fields
        private NetPeer _peer;
        private IPool<NetOutgoingMessageConfiguration> _outgoingMessagePool;
        private ConcurrentQueue<NetOutgoingMessageConfiguration> _outgoingMessages;
        private NetIncomingMessage _im;
        private NetOutgoingMessageConfiguration _omc;
        #endregion

        #region Protected Fields

        #endregion

        #region Public Fields
        public UserCollection Users { get; private set; }
        public GroupCollection Groups { get; private set; }
        public MessageTypeDelegater MessagesTypes { get; private set; }
        #endregion

        #region Constructor
        public Peer(NetPeer peer)
        {
            _peer = peer;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _outgoingMessagePool = provider.GetRequiredService<IPool<NetOutgoingMessageConfiguration>>();
            _outgoingMessages = new ConcurrentQueue<NetOutgoingMessageConfiguration>();
            this.Users = provider.GetRequiredService<UserCollection>();
            this.Groups = provider.GetRequiredService<GroupCollection>();
            this.MessagesTypes = provider.GetRequiredService<MessageTypeDelegater>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.MessagesTypes.TryAdd(NetIncomingMessageType.Data, this.HandleData);
        }

        public override void Dispose()
        {
            base.Dispose();

            _outgoingMessages.Clear();

            this.Users.Dispose();
            this.Groups.Dispose();
            this.MessagesTypes.Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.TryHandleIncomingMessages();

            // Send all outgoing messages...
            while (_outgoingMessages.TryDequeue(out _omc))
            {
                this.SendMessage(_omc);
                _outgoingMessagePool.Put(_omc);
            }
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Read all incoming messages. If there is an error in the process,
        /// skip the message and continue
        /// </summary>
        private void TryHandleIncomingMessages()
        {
#if DEBUG
            // Read any new incoming messages...
            while ((_im = _peer.ReadMessage()) != null)
                this.MessagesTypes.TryInvoke(this, _im.MessageType, _im);
#else
            try
            {
                // Read any new incoming messages...
                while ((_im = _peer.ReadMessage()) != null)
                    this.MessagesTypes.TryInvoke(this, _im.MessageType, _im);
            }
            catch(Exception e)
            {
                this.logger.LogError($"Error handling incoming message. Disconnecting connection & skipping message. ({e.GetType().Name}: {e.Message})");
                _im?.SenderConnection.Disconnect("Goodbye.");
                this.TryHandleIncomingMessages();
            }
#endif
        }
        #endregion

        #region Create & Send Message Methods
        public NetOutgoingMessage CreateMessage(String type, NetDeliveryMethod method, int sequenceChanel, NetConnection recipient, Group group)
        {
            var config = _outgoingMessagePool.Pull(t => new NetOutgoingMessageConfiguration());
            config.Method = method;
            config.SequenceChannel = sequenceChanel;
            config.Recipient = recipient;
            config.Group = group;
            config.Message = _peer.CreateMessage();
            config.Message.Write(group.Id);
            config.Message.Write(type);

            _outgoingMessages.Enqueue(config);

            return config.Message;
        }

        protected abstract void SendMessage(NetOutgoingMessageConfiguration omc);
        #endregion

        protected internal abstract Type GroupType();

        #region MessageType Handlers
        private void HandleData(object sender, NetIncomingMessage im)
        {
            this.Groups.GetOrCreateById(im.ReadGuid()).Messages.Enqueue(im);
        }
        #endregion
    }
}
