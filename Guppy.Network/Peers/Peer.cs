using Guppy.Implementations;
using Guppy.Network.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Initializable, IMessageTarget
    {
        #region Private Fields
        private NetIncomingMessage _im;
        #endregion

        #region Protected Attributes
        protected NetPeer peer;
        protected NetPeerConfiguration config;

        protected Dictionary<String, Action<NetIncomingMessage>> messageHandlers;
        protected NetOutgoingMessageConfigurationPool netOutgoingMessageConfigurationPool;
        protected Queue<NetOutgoingMessageConfiguration> queuedMessages;
        #endregion

        #region Public Fields
        /// <summary>
        /// A shared collection of users within all of the
        /// current peer's groups.
        /// </summary>
        public GlobalUserCollection Users { get; private set; }

        /// <summary>
        /// A collection of all tracked groups within the
        /// current peer
        /// </summary>
        public GroupCollection Groups { get; private set; }
        #endregion

        #region Constructors
        public Peer(NetPeerConfiguration config, NetOutgoingMessageConfigurationPool netOutgoingMessageConfigurationPool, GlobalUserCollection users, GroupCollection groups, IServiceProvider provider) : base(provider)
        {
            this.messageHandlers = new Dictionary<String, Action<NetIncomingMessage>>();
            this.queuedMessages = new Queue<NetOutgoingMessageConfiguration>();
            this.netOutgoingMessageConfigurationPool = netOutgoingMessageConfigurationPool;
            this.config = config;

            this.Users = users;
            this.Groups = groups;
        }
        #endregion

        #region Events
        public event EventHandler<NetIncomingMessage> OnStatusChanged;
        #endregion

        #region Methods
        /// <summary>
        /// Start the internal NetPeer object
        /// </summary>
        public virtual void Start()
        {
            this.TryBoot();
            this.TryPreInitialize();
            this.TryInitialize();
            this.TryPostInitialize();

            this.logger.LogDebug($"Starting Peer<{this.GetType().Name}>...");
            this.peer.Start();
        }

        public void Update()
        {
            this.Flush();

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
                    default:
                        this.logger.LogWarning($"Unhandled incoming message type => {_im.MessageType}");
                        break;
                }
            }
        }

        public NetPeer GetNetPeer()
        {
            return this.peer;
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
                    this.Groups.GetOrCreateById(im.ReadGuid()).HandleMessage(im);
                    break;
                case MessageTarget.Peer:
                    this.HandleMessage(im);
                    break;
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
            this.OnStatusChanged?.Invoke(this, im);
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

        #region IMessageTarget Implementation
        public NetOutgoingMessage CreateMessage(String type, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, int sequenceChanel = 0, NetConnection target = null)
        {
            var om = this.peer.CreateMessage();
            om.Write((Byte)MessageTarget.Peer);
            om.Write(type);

            // Queue up the message for sending next flush
            this.queuedMessages.Enqueue(
                this.netOutgoingMessageConfigurationPool.Pull(om, target, method, sequenceChanel));

            return om;
        }

        public abstract void Flush();

        public void HandleMessage(NetIncomingMessage im)
        {
            String type = im.ReadString();

            if (messageHandlers.ContainsKey(type))
                this.messageHandlers[type].Invoke(im);
            else
                this.logger.LogError($"Unhandled peer message => {type}");
        }

        public void AddMessageHandler(String type, Action<NetIncomingMessage> handler)
        {
            this.messageHandlers[type] = handler;
        }
        #endregion
    }
}
