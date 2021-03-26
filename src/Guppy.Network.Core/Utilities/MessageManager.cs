using Guppy.Network.Contexts;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities
{
    public class MessageManager : IDisposable
    {
        #region Private Fields
        private Action<NetOutgoingMessage> _signer;
        private Func<NetOutgoingMessageContext, NetConnection, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>>, NetOutgoingMessage> _defaultFactory;
        private Dictionary<UInt32, MessageTypeManager> _messageTypes;
        #endregion

        #region Public Properties
        public MessageTypeManager this[UInt32 messageTypeId] => _messageTypes[messageTypeId];
        #endregion

        #region Constructors
        public MessageManager(Action<NetOutgoingMessage> signer, Func<NetOutgoingMessageContext, NetConnection, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>>, NetOutgoingMessage> defaultFactory = null)
        {
            _signer = signer;
            _defaultFactory = defaultFactory;
            _messageTypes = new Dictionary<uint, MessageTypeManager>();
        }

        public void Dispose()
        {
            _messageTypes.Clear();
            _messageTypes = null;
        }
        #endregion

        #region Helper Methods
        public void Add(UInt32 messageType, NetOutgoingMessageContext defaultContext = null, Func<NetOutgoingMessageContext, NetConnection, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>>, NetOutgoingMessage> factory = null)
        {
            _messageTypes.Add(messageType, new MessageTypeManager(messageType, factory ?? _defaultFactory, _signer, defaultContext));
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
