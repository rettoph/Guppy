using Guppy.Utilities.Delegaters;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    /// <summary>
    /// The Network Peer is a service that, by default,
    /// is self managed and will asynchronously update
    /// itself after the TryStart method is first called.
    /// </summary>
    public class Peer : Service
    {
        #region Private Fields
        private NetPeer _peer;
        private NetIncomingMessage _im;
        #endregion

        #region Public Attributes
        public CustomDelegater<NetIncomingMessageType, NetIncomingMessage> MessagesTypes { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.MessagesTypes = new CustomDelegater<NetIncomingMessageType, NetIncomingMessage>();
        }
        #endregion

        #region Frame Methods
        private void TryHandleIncomingMessages()
        {
            try
            {
                // Read any new incoming messages...
                while ((_im = _peer.ReadMessage()) != null)
                    this.MessagesTypes.TryInvoke(this, _im.MessageType, _im);
            }
            catch (Exception e)
            {
                _im?.SenderConnection.Disconnect("Goodbye.");
                this.TryHandleIncomingMessages();
            }
        }
        #endregion
    }
}
