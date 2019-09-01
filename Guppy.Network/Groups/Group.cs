using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Network.Configurations;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Guppy.Pooling.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Utilitites.Delegaters;

namespace Guppy.Network.Groups
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

        #region Internal Attributes
        internal IList<NetConnection> connections;
        #endregion

        #region Public Attributes
        public CreatableCollection<User> Users { get; private set; }
        public MessageDelegater Messages { get; private set; }
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

            this.connections = new List<NetConnection>();

            this.Messages = provider.GetRequiredService<MessageDelegater>();
        }

        public override void Dispose()
        {
            base.Dispose();

            this.connections.Clear();

            this.Users.Dispose();
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
