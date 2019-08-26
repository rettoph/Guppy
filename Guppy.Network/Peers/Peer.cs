using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Configurations;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Target
    {
        #region Protected Fields
        internal Queue<NetOutgoingMessageConfiguration> outgoingMessages;
        #endregion

        #region Private Fields
        private NetPeer _peer;
        #endregion

        #region Constructor
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _peer = provider.GetRequiredService<NetPeer>();
            this.outgoingMessages = new Queue<NetOutgoingMessageConfiguration>();
        }
        #endregion

        #region Target Implementation
        #endregion
    }
}
