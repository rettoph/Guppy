using Guppy.Network.Contexts;
using Guppy.Network.Delegates;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities
{
    public class MessageTypeManager<TMessageTypeId>
    {
        #region Private Fields
        private MessageFactoryDelegate _factory;
        private Action<NetOutgoingMessage, TMessageTypeId> _signer;
        #endregion

        #region Public Properties
        public TMessageTypeId Id { get; private set; }
        public NetOutgoingMessageContext DefaultContext { get; private set; }
        #endregion

        #region Events
        public event OnEventDelegate<MessageTypeManager<TMessageTypeId>, NetIncomingMessage> OnRead;
        public event OnEventDelegate<MessageTypeManager<TMessageTypeId>, NetOutgoingMessage> OnWrite;
        #endregion

        #region Constructors
        public MessageTypeManager(TMessageTypeId id, MessageFactoryDelegate factory, Action<NetOutgoingMessage, TMessageTypeId> signer, NetOutgoingMessageContext defaultContext = null)
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
            this.OnWrite?.Invoke(this, om);
        }

        public void TryRead(NetIncomingMessage im)
        {
            this.OnRead?.Invoke(this, im);
        }


        /// <summary>
        /// Create and write a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type, signed, and enqueue to be sent.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="context"></param>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        public void Create(Action<NetOutgoingMessage> writer, NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
        {
            void SignAndWrite(NetOutgoingMessage om)
            {
                _signer(om, this.Id);

                this.TryWrite(om);

                writer(om);
            }

            _factory(SignAndWrite, context, recipients);
        }

        /// <summary>
        /// Create and write a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type, signed, and enqueue to be sent.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        public void Create(NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients)
        {
            void SignAndWrite(NetOutgoingMessage om)
            {
                _signer(om, this.Id);

                this.TryWrite(om);
            }

            _factory(SignAndWrite, context, recipients);
        }

        /// <summary>
        /// Create & return a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type & enqueue to be sent.
        /// </summary>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public void Create(IEnumerable<NetConnection> recipients)
            => this.Create(this.DefaultContext, recipients);

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
            => this.Create(writer, this.DefaultContext, recipients);

        /// <summary>
        /// Create & return a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type & enqueue to be sent.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public void Create(NetOutgoingMessageContext context, IPipe recipients)
            => this.Create(context, recipients.Users.Connections);

        /// <summary>
        /// Create & return a new <see cref="NetOutgoingMessage"/> instance configured 
        /// for the current message type & enqueue to be sent.
        /// </summary>
        /// <param name="recipients">A list of connections who should be reciving the current message.</param>
        /// <returns></returns>
        public void Create(IPipe recipients)
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
        public void Create(Action<NetOutgoingMessage> writer, NetOutgoingMessageContext context, IPipe recipients)
            => this.Create(writer, context, recipients.Users.Connections);

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
            => this.Create(writer, this.DefaultContext, recipients.Users.Connections);
        #endregion
    }
}
