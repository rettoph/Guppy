using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities
{
    public class MessageManager : IDisposable
    {
        #region Private Fields
        private Dictionary<UInt32, MessageTypeManager> _messageTypes;
        #endregion

        #region Public Properties
        public Action<NetOutgoingMessage> Signer { get; set; }
        public Func<NetOutgoingMessageContext, IEnumerable<NetConnection>, NetOutgoingMessage> DefaultFactory { get; set; }
        #endregion

        #region Public Properties
        public MessageTypeManager this[UInt32 messageTypeId] => _messageTypes[messageTypeId];
        #endregion

        #region Constructors
        public MessageManager()
        {
            _messageTypes = new Dictionary<uint, MessageTypeManager>();
        }

        public void Dispose()
        {
            _messageTypes.Clear();
            _messageTypes = null;
        }
        #endregion

        #region Helper Methods
        public void Add(UInt32 messageType, NetOutgoingMessageContext defaultContext = null, Func<NetOutgoingMessageContext, IEnumerable<NetConnection>, NetOutgoingMessage> factory = null)
        {
            _messageTypes.Add(messageType, new MessageTypeManager(messageType, factory ?? this.DefaultFactory, this.Signer, defaultContext));
        }

        public void Remove(UInt32 messageType)
        {
            _messageTypes.Remove(messageType);
        }

        /// <summary>
        /// Read a specific incoming message with the assumtion that is is
        /// properly signed.
        /// </summary>
        /// <param name="im"></param>
        public void Read(NetIncomingMessage im)
            => _messageTypes[im.ReadUInt32()].TryRead(im);
        #endregion
    }
}
