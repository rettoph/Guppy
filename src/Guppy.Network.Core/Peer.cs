using Guppy.DependencyInjection;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    /// <summary>
    /// The primary peer class, used as a wrapper for all client/server connections.
    /// </summary>
    public class Peer : Service
    {
        #region Private Fields
        private EventBasedNetListener _listener;
        private NetManager _manager;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _listener);
            provider.Service(out _manager);

            _listener.NetworkReceiveEvent += this.HandleNetworkReceiveEvent;
        }
        #endregion

        #region Connect Methods
        public void Connect(String target, Int32 port, String key)
        {
            _manager.Connect(target, port, key);
        }
        #endregion

        #region Start Methods

        /// <summary>
        /// Start logic thread and listening on available port
        /// </summary>
        public void Start()
        {
            _manager.Start();
        }


        /// <summary>
        /// Start logic thread and listening on selected port
        /// </summary>
        /// <param name="port">port to listen</param>
        public void Start(Int32 port)
        {
            _manager.Start(port);
        }
        #endregion

        #region Event Methods
        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
