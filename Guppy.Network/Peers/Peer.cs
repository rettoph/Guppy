using Lidgren.Network;
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
        #endregion

        #region Constructors
        public Peer(IServiceProvider provider)
        {
            _started = false;
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

            this.peer.Start();

            _started = true;
        }
        #endregion
    }
}
