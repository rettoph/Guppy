using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities.Messages
{
    /// <summary>
    /// Simple class used to read and create outgoing messages.
    /// </summary>
    public class MessageManager
    {
        #region Private Fields
        private Dictionary<UInt32, Func<NetIncomingMessage, Boolean>> _messageHandlers;
        private Func<NetDeliveryMethod, Int32, NetConnection, NetOutgoingMessage> _createMessage;
        #endregion

        #region Constructor
        public MessageManager(Func<NetDeliveryMethod, Int32, NetConnection, NetOutgoingMessage> createMessage)
        {
            _createMessage = createMessage;
            _messageHandlers = new Dictionary<UInt32, Func<NetIncomingMessage, Boolean>>();
        }
        #endregion

        #region Events
        public OnEventDelegate<MessageManager, NetOutgoingMessage> OnCreated;
        public ValidateEventDelegate<MessageManager, NetIncomingMessage> ValidateRead;
        #endregion

        #region Helper Methods 
        /// <summary>
        /// Read & process an incoming message
        /// (Assuming that all data was signed for)
        /// </summary>
        /// <param name="im"></param>
        public void Read(NetIncomingMessage im)
        {
            if(this.ValidateRead.Validate(this, im, true))
                while (im.Position < im.LengthBits)
                { // Read the entire message...
                    var id = im.ReadUInt32();
                    if (!_messageHandlers[id](im))
                        break;
                }
        }

        /// <summary>
        /// Set the incoming message handler when
        /// the inputed type is recieved.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        public void Set(UInt32 type, Action<NetIncomingMessage> reader)
            => this.Set(type, im =>
            {
                reader(im);
                return true;
            });
        /// <summary>
        /// Set the incoming message handler when
        /// the inputed type is recieved.
        /// 
        /// Return false if the MessageManager should
        /// stop reading after this message.
        /// 
        /// Return true if theres a chance of multiple
        /// message types packed into a single message.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        public void Set(UInt32 type, Func<NetIncomingMessage, Boolean> reader)
            => _messageHandlers[type] = reader;

        /// <summary>
        /// Remove a reader delegate previously bound to the 
        /// the recieved message type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        public void Remove(UInt32 type)
            => _messageHandlers.Remove(type);

        /// <summary>
        /// Build a brand new message specifically designed to
        /// reach this message manager on all reciving peers.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="sequenceChannel"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public NetOutgoingMessage Create(NetDeliveryMethod method, Int32 sequenceChannel, NetConnection recipient = null)
            => _createMessage(method, sequenceChannel, recipient);
        #endregion
    }
}
