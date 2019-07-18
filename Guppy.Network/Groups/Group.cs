using Guppy.Network.Collections;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Microsoft.Extensions.Logging;
using Guppy.Network.Interfaces;
using Guppy.Network.Configurations;

namespace Guppy.Network.Groups
{
    /// <summary>
    /// Groups represent a collection of users who can privately
    /// send messages to other users within the same group.
    /// </summary>
    public abstract class Group : NetworkObject, IMessageTarget
    {
        #region Private Methods
        private NetPeer _netPeer;
        private Peer _peer;
        private NetIncomingMessage _im;
        private String _type;
        private Queue<NetIncomingMessage> _recievedMessages;
        #endregion

        #region Protected Attributes
        protected Action updateMessages;

        protected Dictionary<String, Action<NetIncomingMessage>> messageHandlers;
        protected NetOutgoingMessageConfigurationPool netOutgoingMessageConfigurationPool;
        protected Queue<NetOutgoingMessageConfiguration> queuedMessages;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The known users in the current game
        /// </summary>
        public UserCollection Users { get; private set; }
        #endregion

        #region Constructor
        public Group(Guid id, Peer peer, NetPeer netPeer, NetOutgoingMessageConfigurationPool netOutgoingMessageConfigurationPool, IServiceProvider provider) : base(id, provider)
        {
            _peer = peer;
            _netPeer = netPeer;
            _recievedMessages = new Queue<NetIncomingMessage>();

            this.messageHandlers = new Dictionary<String, Action<NetIncomingMessage>>();
            this.netOutgoingMessageConfigurationPool = netOutgoingMessageConfigurationPool;
            this.queuedMessages = new Queue<NetOutgoingMessageConfiguration>();

            this.Users = new UserCollection();
        }
        #endregion

        #region Frame Methods
        public virtual void Update()
        {
            this.Flush();

            while(_recievedMessages.Count > 0)
            {
                _im = _recievedMessages.Dequeue();
                _type = _im.ReadString();

                if (this.messageHandlers.ContainsKey(_type))
                    this.messageHandlers[_type].Invoke(_im);
                else
                    this.logger.LogWarning($"Unhandled Group<{this.GetType().Name}>({this.Id}) message => '{_type}'");
            }
        }
        #endregion

        #region IMessageTarget Implementation
        public NetOutgoingMessage CreateMessage(string type, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, int sequenceChanel = 0, NetConnection target = null)
        {
            var om = _netPeer.CreateMessage();
            om.Write((Byte)MessageTarget.Group);
            om.Write(this.Id);
            om.Write(type);

            // Queue up the message for sending next flush
            this.queuedMessages.Enqueue(
                this.netOutgoingMessageConfigurationPool.Pull(om, target, method, sequenceChanel));

            return om;
        }

        public abstract void Flush();

        public void HandleMessage(NetIncomingMessage im)
        {
            _recievedMessages.Enqueue(im);
        }

        public void AddMessageHandler(string type, Action<NetIncomingMessage> handler)
        {
            this.messageHandlers[type] = handler;
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            _recievedMessages.Clear();

            this.messageHandlers.Clear();

            while (queuedMessages.Count > 0)
                this.netOutgoingMessageConfigurationPool.Put(queuedMessages.Dequeue());
        }
    }
}
