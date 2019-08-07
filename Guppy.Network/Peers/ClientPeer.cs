using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Configurations;
using Guppy.Utilities.Pools;
using Lidgren.Network;

namespace Guppy.Network.Peers
{
    /// <summary>
    /// Peer designed specifically for client side
    /// communications.
    /// </summary>
    public class ClientPeer : Peer
    {
        #region Private Fields
        private NetClient _client;
        #endregion

        #region Constructor
        public ClientPeer(NetClient client, Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool) : base(client, outgoingMessageConfigurationPool)
        {
            _client = client;
        }
        #endregion

        #region Target Implementation
        public override void SendMessage(NetOutgoingMessageConfiguration om)
        {
            _client.SendMessage(om.Message, _client.ServerConnection, om.Method, om.SequenceChannel);
        }
        #endregion
    }
}
