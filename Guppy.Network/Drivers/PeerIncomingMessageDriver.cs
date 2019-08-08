using Guppy.Attributes;
using Guppy.Implementations;
using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Drivers
{
    /// <summary>
    /// Simple driver meant to read raw incoming messages
    /// within the peer and invoke custom events for said
    /// messages.
    /// </summary>
    [IsDriver(typeof(Peer), 10)]
    public class PeerIncomingMessageDriver : Driver<Peer>
    {
        #region Private Fields
        private NetPeer _peer;
        private NetIncomingMessage _im;
        #endregion

        #region Constructor
        public PeerIncomingMessageDriver(NetPeer peer)
        {
            _peer = peer;
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            // Register the message related events...
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:data");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:error");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:status-changed");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:unconnected-data");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:connection-approval");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:reciept");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:discovery-request");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:discovery-response");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:verbose-debug-message");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:debug-message");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:warning-message");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:error-message");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:nat-introduction-success");
            this.parent.Events.TryRegisterDelegate<NetIncomingMessage>("recieved:connection-latency-updated");
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            while((_im = _peer.ReadMessage()) != null)
            { // Read all incoming messages...
                switch(_im.MessageType) {
                    case NetIncomingMessageType.Data:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:data", _im);
                        break;
                    case NetIncomingMessageType.Error:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:error", _im);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:status-changed", _im);
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:unconnected-data", _im);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:connection-approval", _im);
                        break;
                    case NetIncomingMessageType.Receipt:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:reciept", _im);
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:discovery-request", _im);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:discovery-response", _im);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:verbose-debug-message", _im);
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:debug-message", _im);
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:warning-message", _im);
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:error-message", _im);
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:nat-introduction-success", _im);
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        this.parent.Events.Invoke<NetIncomingMessage>("recieved:connection-latency-updated", _im);
                        break;
                }
            }
        }
        #endregion
    }
}
