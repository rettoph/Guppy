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
using Microsoft.Xna.Framework;
using Guppy.Network.Security.Collections;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Frameable
    {
        #region Private Fields
        private NetPeer _peer;
        private IPool<NetOutgoingMessageConfiguration> _outgoingMessagePool;
        private NetIncomingMessage _im;
        #endregion

        #region Protected Fields
        protected Queue<NetOutgoingMessageConfiguration> outgoingMessages;
        #endregion

        #region Public Fields
        public UserCollection Users { get; private set; }
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
            this.Users = this.provider.GetRequiredService<UserCollection>();
            this.Groups = this.provider.GetRequiredService<GroupCollection>();
            this.Messages = this.provider.GetRequiredService<PeerMessageDelegater>();
        }

        public override void Dispose()
        {
            base.Dispose();

            this.outgoingMessages.Clear();

            this.Users.Dispose();
            this.Groups.Dispose();
            this.Messages.Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            while((_im = _peer.ReadMessage()) != null)
                this.Messages.Invoke(_im.MessageType, this, _im);
        }
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
