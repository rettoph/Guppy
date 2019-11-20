using Guppy.Network.Configurations;
using Guppy.Network.Security;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities.Delegaters;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.Concurrent;

namespace Guppy.Network.Utilitites.Delegaters
{
    /// <summary>
    /// A message delegater can create, send, and recieve
    /// network messages.
    /// </summary>
    public abstract class MessageDelegater : CustomDelegater<String, NetIncomingMessage>
    {
        #region Private Fields
        private NetPeer _peer;
        private ConcurrentQueue<NetIncomingMessage> _recieved;
        private ConcurrentQueue<NetOutgoingMessageConfiguration> _outgoing;
        private IPool<NetOutgoingMessageConfiguration> _outgoingPool;

        private NetOutgoingMessageConfiguration _omc;
        private NetIncomingMessage _im;
        #endregion

        #region Protected Attributes
        protected ILogger logger { get; private set; }
        #endregion

        #region Constructor
        public MessageDelegater(IPool<NetOutgoingMessageConfiguration> outgoingMessagePool, NetPeer peer, ILogger logger)
        {
            _peer = peer;
            _outgoingPool = outgoingMessagePool;
            _outgoing = new ConcurrentQueue<NetOutgoingMessageConfiguration>();
            _recieved = new ConcurrentQueue<NetIncomingMessage>();

            this.logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send all created messages
        /// </summary>
        public void SendAll()
        {
            // First, send all created messages
            if (this.CanSend())
            { // If we can currently send messages...
                while (_outgoing.Any())
                    if (_outgoing.TryDequeue(out _omc))
                    {
                        this.Send(_peer, _omc);
                        _outgoingPool.Put(_omc);
                    }
            }
            else
            { // If messages cannot currently be sent
                // Remove all messages and return them into the pool
                while (_outgoing.Any())
                    if (_outgoing.TryDequeue(out _omc))
                        _outgoingPool.Put(_omc);
            }
        }

        /// <summary>
        /// Read all recieved messages
        /// </summary>
        public void ReadAll()
        {
            // Next, handle all recieved messages
            while (_recieved.Any())
                if (_recieved.TryDequeue(out _im))
                    this.Invoke<NetIncomingMessage>(this, _im.ReadString(), _im);
        }

        public override void Dispose()
        {
            base.Dispose();

            _outgoing.Clear();
            _recieved.Clear();
        }

        /// <summary>
        /// Enqueue an incoming message to be read next Flush.
        /// </summary>
        /// <param name="im"></param>
        public void Enqueue(NetIncomingMessage im)
        {
            _recieved.Enqueue(im);
        }
        #endregion

        #region Create Methods
        public virtual NetOutgoingMessage Create(String type, NetDeliveryMethod method, int sequenceChanel, NetConnection recipient = null)
        {
            var config = _outgoingPool.Pull(t => new NetOutgoingMessageConfiguration());

            // Setup the config...
            config.Method = method;
            config.SequenceChannel = sequenceChanel;
            config.Recipient = recipient;
            config.Message = _peer.CreateMessage();
            this.Sign(config.Message, type);

            // Enqueue the message
            _outgoing.Enqueue(config);

            return config.Message;
        }
        public NetOutgoingMessage Create(String type, NetDeliveryMethod method, int sequenceChanel, User recipient)
        {
            return this.Create(type, method, sequenceChanel, recipient?.Connection);
        }

        protected virtual Boolean CanSend()
        {
            return _peer.Connections.Any();
        }
        #endregion

        #region Delegator Overrides
        protected override void Invoke<T>(object sender, string key, T arg)
        {
#if DEBUG
            try
            {
                base.Invoke(sender, key, arg);
            }
            catch (KeyNotFoundException e)
            {
                this.logger.LogWarning($"Unhandled Message recieved => '{key}'");
            }
#else
            base.Invoke(sender, key, arg);
#endif
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Send the selected message through the inputed peer
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="config"></param>
        protected abstract void Send(NetPeer peer, NetOutgoingMessageConfiguration config);
        /// <summary>
        /// Sign the inputed message with the given type as needed
        /// </summary>
        /// <param name="om"></param>
        /// <param name="type"></param>
        protected abstract void Sign(NetOutgoingMessage om, String type);
        #endregion
    }
}
