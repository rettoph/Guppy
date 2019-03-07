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
        #endregion

        #region Protected Attributes
        protected NetPeer peer;
        protected NetPeerConfiguration config;
        protected ILogger logger;
        #endregion

        #region Public Attributes
        public NetPeerConfiguration Configuration { get; private set; }
        #endregion

        #region Constructors
        public Peer(NetPeerConfiguration config, ILogger logger)
        {
            _started = false;
            this.config = config;
            this.logger = logger;
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
        #endregion
    }
}
