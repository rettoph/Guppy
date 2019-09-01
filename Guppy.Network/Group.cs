using Guppy.Collections;
using Guppy.Network.Configurations;
using Guppy.Network.Peers;
using Guppy.Network.Security;
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

        #region Public Attributes
        public CreatableCollection<User> Users { get; private set; }
        #endregion

        #region Constructor
        public Group(CreatableCollection<User> users, Peer peer)
        {
            _peer = peer;

            this.Users = users;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);
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
