using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.Network.Utilities
{
    public sealed class MessageDelegater
    {
        #region Private Fields
        private Dictionary<UInt32, MessageDelegate> _messageDelegates;
        #endregion

        #region Delegates
        public delegate void MessageDelegate(Messageable sender, NetIncomingMessage im);
        #endregion

        #region Helper Methods
        /// <summary>
        /// Add a new message delegate with a human readable key.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageDelegate"></param>
        public void Add(String messageType, MessageDelegate messageDelegate)
        {
            this.Add(xxHash.CalculateHash(Encoding.UTF8.GetBytes(messageType)), messageDelegate);
        }
        /// <summary>
        /// Add a new message delegate with the human reable key encoded.
        /// </summary>
        /// <param name="messageTypeId"></param>
        /// <param name="messageDelegate"></param>
        public void Add(UInt32 messageTypeId, MessageDelegate messageDelegate)
        {
            if (_messageDelegates.ContainsKey(messageTypeId))
                _messageDelegates[messageTypeId] += messageDelegate;
            else
                _messageDelegates[messageTypeId] = messageDelegate;
        }

        /// <summary>
        /// Remove an existing message delegate bound to a human readable key.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageDelegate"></param>
        public void Remove(String messageType, MessageDelegate messageDelegate)
        {
            this.Remove(xxHash.CalculateHash(Encoding.UTF8.GetBytes(messageType)), messageDelegate);
        }
        /// <summary>
        /// Remove an existing message delegate bound to an encoded human readable key.
        /// </summary>
        /// <param name="messageTypeId"></param>
        /// <param name="messageDelegate"></param>
        public void Remove(UInt32 messageTypeId, MessageDelegate messageDelegate)
        {
            _messageDelegates[messageTypeId] -= messageDelegate;
        }

        /// <summary>
        /// Take the recieved message and invoke and delegates attached to it
        /// </summary>
        /// <param name="im"></param>
        internal void Handle(Messageable sender, NetIncomingMessage im)
        {
            _messageDelegates[im.ReadUInt32()]?.Invoke(sender, im);
        }
        #endregion
    }
}
