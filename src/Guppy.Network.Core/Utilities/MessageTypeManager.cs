using Guppy.Events.Delegates;
using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities
{
    public sealed class MessageTypeManager
    {
        #region Private Fields
        private Func<NetOutgoingMessageContext, IEnumerable<NetConnection>, NetOutgoingMessage> _factory;
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
        public MessageTypeManager(UInt32 id, Func<NetOutgoingMessageContext, IEnumerable<NetConnection>, NetOutgoingMessage> factory, Action<NetOutgoingMessage> signer, NetOutgoingMessageContext defaultContext = null)
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
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public NetOutgoingMessage Create(NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
        {
            var message = _factory(context, recipients);
            this.TryWrite(message);

            return message;
        }
        /// <summary>
        /// Create & return a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type & enqueue to be sent.
        /// </summary>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public NetOutgoingMessage Create(IEnumerable<NetConnection> recipients)
            => this.Create(this.DefaultContext, recipients);


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
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public void Create(NetOutgoingMessageContext context, Action<NetOutgoingMessage> writer, IEnumerable<NetConnection> recipients)
            => writer(this.Create(context, recipients));

        /// <summary>
        /// <para>Create a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type, invoke the custom writer, then
        /// enqueue to be sent.</para>
        /// 
        /// <para>Note, the recieving <see cref="MessageTypeManager"/> must still be configured to process
        /// the incoming message.</para>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public void Create(Action<NetOutgoingMessage> writer, IEnumerable<NetConnection> recipients)
            => this.Create(this.DefaultContext, writer, recipients);

        /// <summary>
        /// Create & return a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type & enqueue to be sent.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public NetOutgoingMessage Create(NetOutgoingMessageContext context, IPipe recipients)
            => Create(context, recipients.Users.Connections);

        /// <summary>
        /// Create & return a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type & enqueue to be sent.
        /// </summary>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public NetOutgoingMessage Create(IPipe recipients)
            => this.Create(this.DefaultContext, recipients.Users.Connections);


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
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public void Create(NetOutgoingMessageContext context, Action<NetOutgoingMessage> writer, IPipe recipients)
            => writer(this.Create(context, recipients.Users.Connections));

        /// <summary>
        /// <para>Create a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type, invoke the custom writer, then
        /// enqueue to be sent.</para>
        /// 
        /// <para>Note, the recieving <see cref="MessageTypeManager"/> must still be configured to process
        /// the incoming message.</para>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public void Create(Action<NetOutgoingMessage> writer, IPipe recipients)
            => this.Create(this.DefaultContext, writer, recipients.Users.Connections);
        #endregion
    }
}
