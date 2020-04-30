using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using Guppy.DependencyInjection;
using Lidgren.Network;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Messageable
    {
        #region Private Fields
        private NetPeer _peer;
        private Thread _updateThread;
        private Boolean _running;
        private NetIncomingMessage _im;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The peer's update interval in milliseconds.
        /// </summary>
        public Int32 Interval { get; set; } = 32;
        public ReadOnlyDictionary<NetIncomingMessageType, MessageTypeDelegate> MessageTypeDelegates { get; private set; }
        #endregion

        #region Delegates
        public delegate void MessageTypeDelegate(NetIncomingMessage im);
        #endregion

        #region Constructor
        public Peer()
        {

        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.MessageTypeDelegates = new ReadOnlyDictionary<NetIncomingMessageType, MessageTypeDelegate>(((NetIncomingMessageType[])Enum.GetValues(typeof(NetIncomingMessageType))).ToDictionary(
                keySelector: mt => mt,
                elementSelector: mt => default(MessageTypeDelegate)));

            _peer = this.GetPeer(provider);
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.MessageTypeDelegates = null;
        }
        #endregion

        #region Helper Methods
        public void Start()
        {
            lock (this)
            {
                if (_running)
                    throw new Exception("Unable to Start when already running.");

                _peer.Start();
                _updateThread = new Thread(new ThreadStart(this.UpdateLoop));
                _running = true;
            }
        }

        public void Stop()
        {
            lock (this)
            {
                if (!_running)
                    throw new Exception("Unable to stop when not running");

                _running = false;
            }
        }

        protected abstract NetPeer GetPeer(ServiceProvider provider);
        #endregion

        #region Frame Methods
        public void UpdateLoop()
        {
            while(_running)
            {
                this.Update();
                Thread.Sleep(this.Interval);
            }
        }

        public override void Update()
        {
            while ((_im = _peer.ReadMessage()) != null)
                this.MessageTypeDelegates[_im.MessageType]?.Invoke(_im);

            base.Update();
        }
        #endregion
    }
}
