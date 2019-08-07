using Guppy.Extensions.DependencyInjection;
using Guppy.Implementations;
using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Implementations;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    /// <summary>
    /// The core peer class.
    /// This defines peer unspecific
    /// functionality. Client or server 
    /// specific functions will be implemented
    /// within the ClientPeer and ServerPeer
    /// classes.
    /// </summary>
    public abstract class Peer : Target
    {
        #region Private Fields
        private NetPeer _peer;
        #endregion

        #region Constructor
        public Peer(NetPeer peer, Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool) : base(peer, outgoingMessageConfigurationPool)
        {
            _peer = peer;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            provider.SetConfigurationValue("peer", this);
        }
        #endregion

        #region Helper Methods
        public virtual void Start()
        {
            this.logger.LogDebug($"Starting Peer<{_peer.GetType().Name}>...");
            _peer.Start();
        }

        public virtual void Shutdown(String bye)
        {
            this.logger.LogDebug($"Shutting down Peer<{_peer.GetType().Name}>...");
            _peer.Shutdown(bye);
        }
        #endregion

        #region Target Methods
        protected override void ConfigureMessage(NetOutgoingMessage om)
        {
            om.Write((Byte)MessageTarget.Peer);
        }
        #endregion
    }
}
