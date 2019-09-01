using Guppy.Network.Configurations;
using Guppy.Network.Peers;
using Guppy.Pooling.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    /// <summary>
    /// Groups represent collections of connections that
    /// are capable of sending messages back and fourth
    /// to the connected peer.
    /// </summary>
    public class Group : Frameable
    {
        #region Private Fields
        private Peer _peer;
        #endregion

        #region Internal Fields
        internal HashSet<NetConnection> connections;
        #endregion

        #region Constructor
        public Group(Peer peer)
        {
            _peer = peer;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.connections = new HashSet<NetConnection>();
        }
        #endregion

        #region CreateMessage methods
        public NetOutgoingMessage CreateMessage(String type, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, int sequenceChanel = 0, NetConnection recipient = null)
        {
            return _peer.CreateMessage(
                type: type,
                method: method,
                sequenceChanel: sequenceChanel,
                recipient: recipient,
                group: this);
        }
        #endregion
    }
}
