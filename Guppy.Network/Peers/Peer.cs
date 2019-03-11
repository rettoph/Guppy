using Guppy.Network.Collections;
using Guppy.Network.Enums;
using Guppy.Network.Extensions;
using Guppy.Network.Groups;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    public abstract class Peer
    {
        #region Private Fields
        private Boolean _started;
        private NetIncomingMessage _im;
        #endregion

        #region Protected Attributes
        protected NetPeer peer;
        protected NetPeerConfiguration config;
        protected ILogger logger;
        #endregion

        #region Public Fields
        /// <summary>
        /// A shared collection of users within all of the
        /// current peer's groups.
        /// </summary>
        public UserCollection Users { get; private set; }

        /// <summary>
        /// A collection of all tracked groups within the
        /// current peer
        /// </summary>
        public GroupCollection Groups { get; private set; }
        #endregion

        #region Constructors
        public Peer(NetPeerConfiguration config, ILogger logger)
        {
            _started = false;
            this.config = config;
            this.logger = logger;

            this.Users = new UserCollection();
            this.Groups = new GroupCollection(this.CreateGroup);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start the internal NetPeer object
        /// </summary>
        public virtual void Start()
        {
            if (_started)
                throw new Exception("Unable to start peer. Peer already started.");

            this.logger.LogDebug($"Starting Peer<{this.GetType().Name}>...");
            this.peer.Start();

            _started = true;
        }

        public void Update()
        {
            // Flush the message buffer
            this.peer.FlushSendQueue();

            while((_im = this.peer.ReadMessage()) != null)
            { // Read any and all incoming messages...
                switch (_im.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        this.HandleData(_im);
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        this.HandleConnectionLatencyUpdated(_im);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        this.HandleConnectionApproval(_im);
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        this.HandleUnconnectedData(_im);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        this.HandleStatusChanged(_im);
                        break;
                    case NetIncomingMessageType.Error:
                        this.HandleError(_im);
                        break;
                    case NetIncomingMessageType.Receipt:
                        this.HandleReceipt(_im);
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        this.HandleDiscoveryRequest(_im);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        this.HandleDiscoveryResponse(_im);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        this.HandleVerboseDebugMessage(_im);
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        this.HandleDebugMessage(_im);
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        this.HandleWarningMessage(_im);
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        this.HandleErrorMessage(_im);
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        this.HandleNatIntroductionSuccess(_im);
                        break;
                }
            }
        }

        protected internal abstract Group CreateGroup(Guid id);
        #endregion

        #region Send Message Methods
        public void SendMessage(NetOutgoingMessage om, NetConnection recipient, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, Int32 sequenceChannel = 0)
        {
            this.peer.SendMessage(om, recipient, method, sequenceChannel);
        }
        public void SendMessage(NetOutgoingMessage om, List<NetConnection> recipients, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, Int32 sequenceChannel = 0)
        {
            this.peer.SendMessage(om, recipients, method, sequenceChannel);
        }
        #endregion

        #region MessageType Handlers
        /// <summary>
        /// Custom data handler
        /// </summary>
        /// <param name="im"></param>
        protected virtual void HandleData(NetIncomingMessage im)
        {
            switch ((MessageTarget)im.ReadByte())
            {
                case MessageTarget.Group:
                    this.Groups.GetOrCreateById(im.ReadGuid()).HandleData(im);
                    break;
                case MessageTarget.Peer:
                    throw new Exception("Peer messages are not yet supported.");
                case MessageTarget.User:
                    throw new Exception("User messages are not yet supported.");
            }
        }

        protected virtual void HandleError(NetIncomingMessage im)
        {
            this.logger.LogError($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleStatusChanged(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.SenderConnection.Status}");
        }

        protected virtual void HandleUnconnectedData(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleConnectionApproval(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType}...");
        }

        protected virtual void HandleReceipt(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleDiscoveryRequest(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleDiscoveryResponse(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleVerboseDebugMessage(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleDebugMessage(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleWarningMessage(NetIncomingMessage im)
        {
            this.logger.LogWarning(im.ReadString());
        }

        protected virtual void HandleErrorMessage(NetIncomingMessage im)
        {
            this.logger.LogError($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleNatIntroductionSuccess(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void HandleConnectionLatencyUpdated(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }
        #endregion

        #region CreateMessage Methods
        protected internal NetOutgoingMessage CreateMessage()
        {
            return this.peer.CreateMessage();
        }
        protected internal NetOutgoingMessage CreateMessage(MessageTarget target)
        {
            var om = this.CreateMessage();
            om.Write((Byte)target);

            return om;
        }
        #endregion
    }
}
