using Guppy.Extensions.DependencyInjection;
using Guppy.Implementations;
using Lidgren.Network;
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
    public abstract class Peer : Reusable
    {
        #region Private Fields
        private NetPeer _peer;
        #endregion

        #region Constructor
        public Peer(NetPeer peer)
        {

        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Save internal scope values...
            provider.SetConfigurationValue("peer", this);
            provider.SetConfigurationValue("net-peer", _peer);
        }
        #endregion
    }
}
