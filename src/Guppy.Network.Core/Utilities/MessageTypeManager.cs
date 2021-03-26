using Guppy.Events.Delegates;
using Guppy.Network.Contexts;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities
{
    public sealed class MessageTypeManager
    {
        #region Private Fields
        private Func<NetOutgoingMessageContext, NetConnection, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>>, NetOutgoingMessage> _factory;
        private Action<NetOutgoingMessage> _signer;
        #endregion

        #region Public Properties
        public UInt32 Id { get; private set; }
        public NetOutgoingMessageContext DefaultContext { get; private set; }
        #endregion

        #region Events
        public event OnEventDelegate<MessageTypeManager, NetIncomingMessage> OnRead;
        public event OnEventDelegate<MessageTypeManager, NetOutgoingMessage> OnWrite;
        #endregion

        #region Constructors
        public MessageTypeManager(UInt32 id, Func<NetOutgoingMessageContext, NetConnection, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>>, NetOutgoingMessage> factory, Action<NetOutgoingMessage> signer, NetOutgoingMessageContext defaultContext = null)
        {
            this.Id = id;
            this.DefaultContext = defaultContext;

            _factory = factory;
            _signer = signer;
        }
        #endregion

        #region Helper Methods
        public void TryWrite(NetOutgoingMessage om)
        {
            _signer(om);
            om.Write(this.Id);

            this.OnWrite?.Invoke(this, om);
        }

        public void TryRead(NetIncomingMessage im)
        {
            this.OnRead?.Invoke(this, im);
        }

        /// <summary>
        /// Create & return a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type & enqueue to be sent.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="recipient">A specific reciepient for the message. If null the message will be boradcasted through the pipe</param>
        /// <param name="filter">A custom filter for determining which connectios should recieve the specified message.</param>
        /// <returns></returns>
        public NetOutgoingMessage Create(NetOutgoingMessageContext context, NetConnection recipient = null, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter = null)
        {
            var message = _factory(context, recipient, filter);
            this.TryWrite(message);

            return message;
        }
        /// <summary>
        /// Create & return a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type & enqueue to be sent.
        /// </summary>
        /// <param name="recipient">A specific reciepient for the message. If null the message will be boradcasted through the pipe</param>
        /// <param name="filter">A custom filter for determining which connectios should recieve the specified message.</param>
        /// <returns></returns>
        public NetOutgoingMessage Create(NetConnection recipient = null, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter = null)
            => this.Create(this.DefaultContext, recipient, filter);


        /// <summary>
        /// <para>Create a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type, invoke the custom writer, then
        /// enqueue to be sent.</para>
        /// 
        /// <para>Note, the recieving <see cref="MessageTypeManager"/> must still be configured to process
        /// the incoming message.</para>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <param name="recipient">A specific reciepient for the message. If null the message will be boradcasted through the pipe</param>
        /// <param name="filter">A custom filter for determining which connectios should recieve the specified message.</param>
        /// <returns></returns>
        public void Create(NetOutgoingMessageContext context, Action<NetOutgoingMessage> writer, NetConnection recipient = null, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter = null)
            => writer(this.Create(context, recipient, filter));

        /// <summary>
        /// <para>Create a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type, invoke the custom writer, then
        /// enqueue to be sent.</para>
        /// 
        /// <para>Note, the recieving <see cref="MessageTypeManager"/> must still be configured to process
        /// the incoming message.</para>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="recipient">A specific reciepient for the message. If null the message will be boradcasted through the pipe</param>
        /// <param name="filter">A custom filter for determining which connectios should recieve the specified message.</param>
        /// <returns></returns>
        public void Create(Action<NetOutgoingMessage> writer, NetConnection recipient = null, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter = null)
            => this.Create(this.DefaultContext, writer, recipient, filter);
        #endregion
    }
}
