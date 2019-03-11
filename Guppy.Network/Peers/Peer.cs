using Guppy.Network.Collections;
using Guppy.Network.Enums;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    public class Peer
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
        #endregion

        #region Constructors
        public Peer(NetPeerConfiguration config, ILogger logger)
        {
            _started = false;
            this.config = config;
            this.logger = logger;

            this.Users = new UserCollection();
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
            while((_im = this.peer.ReadMessage()) != null)
            { // Read any and all incoming messages...
                switch (_im.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        this.Data(_im);
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        this.ConnectionLatencyUpdated(_im);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        this.ConnectionApproval(_im);
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        this.UnconnectedData(_im);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        this.StatusChanged(_im);
                        break;
                    case NetIncomingMessageType.Error:
                        this.Error(_im);
                        break;
                    case NetIncomingMessageType.Receipt:
                        this.Receipt(_im);
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        this.DiscoveryRequest(_im);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        this.DiscoveryResponse(_im);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        this.VerboseDebugMessage(_im);
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        this.DebugMessage(_im);
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        this.WarningMessage(_im);
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        this.ErrorMessage(_im);
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        this.NatIntroductionSuccess(_im);
                        break;
                }
            }
        }
        #endregion

        #region Send Message Methos
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
        protected virtual void Error(NetIncomingMessage im)
        {
            this.logger.LogError($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void StatusChanged(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.SenderConnection.Status}");
        }

        protected virtual void UnconnectedData(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void ConnectionApproval(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType}...");
        }

        protected virtual void Data(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void Receipt(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void DiscoveryRequest(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void DiscoveryResponse(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void VerboseDebugMessage(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void DebugMessage(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void WarningMessage(NetIncomingMessage im)
        {
            this.logger.LogWarning(im.ReadString());
        }

        protected virtual void ErrorMessage(NetIncomingMessage im)
        {
            this.logger.LogError($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void NatIntroductionSuccess(NetIncomingMessage im)
        {
            this.logger.LogDebug($"{im.MessageType} - {im.ReadString()}");
        }

        protected virtual void ConnectionLatencyUpdated(NetIncomingMessage im)
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
