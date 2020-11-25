using Guppy.DependencyInjection;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Network.Structs;
using Guppy.Network.Utilities;
using Guppy.Network.Extensions.Lidgren;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Network.Utilities.Messages;
using System.Collections.Concurrent;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network
{
    // Create some useful global delegates
    public delegate void NetIncomingMessageDelegate(NetIncomingMessage im);
    public delegate void NetOutgoingMessageDelegate(NetOutgoingMessage om);

    /// <summary>
    /// An Object capable of creating & recieving messages.
    /// </summary>
    public abstract class Messageable : Asyncable
    {
        #region Private Fields
        private NetPeer _peer;
        private NetOutgoingMessageConfiguration _om;
        private NetIncomingMessage _im;
        #endregion

        #region Public Attributes
        public MessageManager Messages { get; private set; }

        /// <summary>
        /// Messages to be read next invocation of Update
        /// </summary>
        public ConcurrentQueue<NetIncomingMessage> IncomingMessages { get; private set; }

        /// <summary>
        /// Messages to be sent out next invocation of Update
        /// </summary>
        public ConcurrentQueue<NetOutgoingMessageConfiguration> OutgoingMessages { get; private set; }
        #endregion

        #region Lifeccyle Methods

        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.OutgoingMessages = new ConcurrentQueue<NetOutgoingMessageConfiguration>();
            this.IncomingMessages = new ConcurrentQueue<NetIncomingMessage>();
            this.Messages = new MessageManager(this.CreateMessage);
        }
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service<NetPeer>(out _peer);
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Send all outbound messages...
            while (this.OutgoingMessages.Any())
                if(this.OutgoingMessages.TryDequeue(out _om))
                    this.Send(_om);

            // Read all inbound messages...
            while (this.IncomingMessages.Any())
                if(this.IncomingMessages.TryDequeue(out _im))
                    this.Messages.Read(_im);
        }
        #endregion

        #region Create Message Methods
        protected abstract void Send(NetOutgoingMessageConfiguration message);
        protected abstract MessageTarget TargetType();
        private NetOutgoingMessage CreateMessage(NetDeliveryMethod method, int sequenceChanel, NetConnection recipient = null)
        {
            var config = new NetOutgoingMessageConfiguration()
            { // Create a brand new message configuration..
                Method = method,
                SequenceChannel = sequenceChanel,
                Recipient = recipient,
                Message = _peer.CreateMessage()
            };

            // Sign the target type and current id.
            config.Message.Write((Byte)this.TargetType());
            config.Message.Write(this.Id);

            // Enqueue the outbound message.
            this.OutgoingMessages.Enqueue(config);

            // Return the newly created message.
            return config.Message;
        }
        #endregion
    }
}
