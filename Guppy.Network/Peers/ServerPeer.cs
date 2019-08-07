using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Guppy.Network.Configurations;
using Guppy.Utilities.Pools;
using Lidgren.Network;

namespace Guppy.Network.Peers
{
    /// <summary>
    /// Peer designed specifically for server side
    /// communications.
    /// </summary>
    public class ServerPeer : Peer
    {
        #region Private Fields
        private EntityCollection _entities;
        private NetServer _server;
        #endregion

        #region Constructor
        public ServerPeer(EntityCollection entities, NetServer server, Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool) : base(server, outgoingMessageConfigurationPool)
        {
            _entities = entities;
            _server = server;
        }
        #endregion

        #region Target Implementation
        public override void SendMessage(NetOutgoingMessageConfiguration om)
        {
            if (om.Target == null)
                _server.SendToAll(om.Message, null, om.Method, om.SequenceChannel);
            else;
                _server.SendMessage(om.Message, om.Target, om.Method, om.SequenceChannel);
        }
        #endregion
    }
}
