using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Configurations;
using Guppy.Pooling.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Collections;
using Guppy.Network.Utilitites.Delegaters;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Frameable
    {
        #region Private Fields
        private NetPeer _peer;
        private IPool<NetOutgoingMessageConfiguration> _outgoingMessagePool;
        #endregion

        #region Protected Fields
        protected Queue<NetOutgoingMessageConfiguration> outgoingMessages;
        #endregion

        #region Public Fields
        public GroupCollection Groups { get; private set; }
        public PeerMessageDelegater Messages { get; private set; }
        #endregion

        #region Constructor
        public Peer(NetPeer peer)
        {
            _peer = peer;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _outgoingMessagePool = provider.GetRequiredService<IPool<NetOutgoingMessageConfiguration>>();
            this.outgoingMessages = new Queue<NetOutgoingMessageConfiguration>();
            this.Groups = this.provider.GetRequiredService<GroupCollection>();
            this.Messages = this.provider.GetRequiredService<PeerMessageDelegater>();
        }
        #endregion

        #region Frame Methods
        #endregion

        #region CreateMessage Methods
        public NetOutgoingMessage CreateMessage(String type, NetDeliveryMethod method, int sequenceChanel, NetConnection recipient, Group group)
        {
            var config = _outgoingMessagePool.Pull(t => new NetOutgoingMessageConfiguration());
            config.Method = method;
            config.SequenceChannel = sequenceChanel;
            config.Recipient = recipient;
            config.Group = group;
            config.Message = _peer.CreateMessage();
            config.Message.Write(group.Id);
            config.Message.Write(type);

            this.outgoingMessages.Enqueue(config);

            return config.Message;
        }
        #endregion
    }
}
