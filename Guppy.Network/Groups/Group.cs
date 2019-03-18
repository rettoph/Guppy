using Guppy.Network.Collections;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions;
using Microsoft.Extensions.Logging;

namespace Guppy.Network.Groups
{
    /// <summary>
    /// Groups represent a collection of users who can privately
    /// send messages to other users within the same group.
    /// </summary>
    public abstract class Group : NetworkObject
    {
        #region Private Methods
        private Peer _peer;
        private Queue<NetIncomingMessage> _ignoredMessageBuffer;
        private Queue<NetIncomingMessage> _messageBuffer;
        private NetIncomingMessage _im;
        private Boolean _ignoreData;
        private String _messageType;
        #endregion

        #region Protected Attributes
        protected MessageHandler internalMessageHandler;
        protected ILogger log;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The known users in the current game
        /// </summary>
        public NetworkObjectCollection<User> Users { get; private set; }

        /// <summary>
        /// When true, any recieved data messages will automatically
        /// be ignored until marked false. Then all recieved messages
        /// will be parsed on the next update.
        /// </summary>
        public Boolean IgnoreData
        {
            get { return _ignoreData; }
            set
            {
                if(value != _ignoreData)
                {
                    _ignoreData = value;
                    if(!_ignoreData)
                    {
                        while (_ignoredMessageBuffer.Count > 0)
                            _messageBuffer.Enqueue(_ignoredMessageBuffer.Dequeue());
                    }
                }
            }
        }
        
        public MessageHandler MessageHandler { get; private set; }
        #endregion

        #region Constructor
        public Group(Guid id, Peer peer, ILogger log) : base(id)
        {
            _peer = peer;
            _ignoredMessageBuffer = new Queue<NetIncomingMessage>();
            _messageBuffer = new Queue<NetIncomingMessage>();

            this.internalMessageHandler = new MessageHandler();

            this.Users = new NetworkObjectCollection<User>(disposeOnRemove: false);
            this.MessageHandler = new MessageHandler();

            this.log = log;

            // By default, groups should ignore data
            this.IgnoreData = false;
        }
        #endregion

        #region Methods
        public void Update()
        {
            // Read any messages in the buffer
            while(_messageBuffer.Count > 0)
            {
                // Call the message handler based on the message data
                _im = _messageBuffer.Dequeue();

                _messageType = _im.ReadString();

                if (this.MessageHandler.ContainsKey(_messageType))
                    this.MessageHandler[_messageType](_im);
                else
                    this.log.LogWarning($"Unhandled message recieved: '{_messageType}'");
            }
        }
        #endregion

       #region Create Message Methods
        protected internal NetOutgoingMessage CreateMessage(MessageType type)
        {
            var om = _peer.CreateMessage(MessageTarget.Group);
            om.Write(this.Id);
            om.Write((Byte)type);

            return om;
        }
        protected internal NetOutgoingMessage CreateMessage(String data, MessageType type)
        {
            var om = this.CreateMessage(type);
            om.Write(data);

            return om;
        }
        protected internal NetOutgoingMessage CreateMessage()
        {
            return this.CreateMessage(MessageType.Data);
        }
        public NetOutgoingMessage CreateMessage(String messageType)
        {
            var om = this.CreateMessage();
            om.Write(messageType);

            return om;
        }
        #endregion

        #region Send Message Methods
        public abstract void SendMesssage(NetOutgoingMessage om, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, Int32 sequenceChannel = 0);
        #endregion

        #region Message handlers
        internal void HandleData(NetIncomingMessage im)
        {
            switch ((MessageType)im.ReadByte())
            {
                case MessageType.Data:
                    _messageBuffer.Enqueue(im);
                    break;
                case MessageType.Internal:
                    _messageType = im.ReadString();

                    if (this.internalMessageHandler.ContainsKey(_messageType))
                        this.internalMessageHandler[_messageType](im);
                    else
                        this.log.LogWarning($"Unhandled internal message recieved: '{_messageType}'");
                    break;
            }
        }
        #endregion
    }
}
