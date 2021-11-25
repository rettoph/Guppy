using Guppy.Network.Contexts;
using Guppy.Network.Delegates;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities
{
    public class ByteMessageManager : MessageManager<Byte>
    {
        protected override Byte ReadMessageTypeId(NetIncomingMessage im)
        {
            return im.ReadByte();
        }

        protected override void WriteMessageTypeId(NetOutgoingMessage om, Byte id)
        {
            om.Write(id);
        }
    }

    public class MessageManager : MessageManager<UInt32>
    {
        protected override UInt32 ReadMessageTypeId(NetIncomingMessage im)
        {
            return im.ReadUInt32();
        }

        protected override void WriteMessageTypeId(NetOutgoingMessage om, UInt32 id)
        {
            om.Write(id);
        }
    }

    public abstract class MessageManager<TMessageTypeId> : IDisposable
    {
        #region Private Fields
        private Dictionary<TMessageTypeId, MessageTypeManager<TMessageTypeId>> _messageTypes;
        #endregion

        #region Public Properties
        /// <summary>
        /// Used to write a custom signature to the start of the message. This is traditionally used
        /// for writting the <see cref="MessageManager{TMessageTypeId}"/>'s owner's Id.
        /// </summary>
        public Action<NetOutgoingMessage> CustomSigner { get; set; }
        public MessageFactoryDelegate DefaultFactory { get; set; }
        #endregion

        #region Public Properties
        public MessageTypeManager<TMessageTypeId> this[TMessageTypeId messageTypeId] => _messageTypes[messageTypeId];
        #endregion

        #region Constructors
        public MessageManager()
        {
            _messageTypes = new Dictionary<TMessageTypeId, MessageTypeManager<TMessageTypeId>>();
        }

        public void Dispose()
        {
            _messageTypes.Clear();
            _messageTypes = null;
        }
        #endregion

        #region Helper Methods
        public void Add(TMessageTypeId messageType, NetOutgoingMessageContext defaultContext = null, MessageFactoryDelegate factory = null)
        {
            _messageTypes.Add(messageType, new MessageTypeManager<TMessageTypeId>(messageType, factory ?? this.DefaultFactory, this.Sign, defaultContext));
        }

        public void Clear()
        {
            _messageTypes.Clear();
        }

        /// <summary>
        /// Read a specific incoming message with the assumtion that is is
        /// properly signed.
        /// </summary>
        /// <param name="im"></param>
        public void Read(NetIncomingMessage im)
            => _messageTypes[this.ReadMessageTypeId(im)].TryRead(im);

        /// <summary>
        /// Used to appropriately sign a message.
        /// </summary>
        /// <param name="om"></param>
        /// <param name="messageTypeId"></param>
        private void Sign(NetOutgoingMessage om, TMessageTypeId messageTypeId)
        {
            this.CustomSigner(om);

            this.WriteMessageTypeId(om, messageTypeId);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Read the current MessageTypeManager id
        /// </summary>
        /// <param name="om"></param>
        protected abstract TMessageTypeId ReadMessageTypeId(NetIncomingMessage im);

        /// <summary>
        /// Write the current MessageTypeManager id
        /// </summary>
        /// <param name="om"></param>
        protected abstract void WriteMessageTypeId(NetOutgoingMessage om, TMessageTypeId messageTypeId);
        #endregion
    }
}
