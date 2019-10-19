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
using Guppy.Network.Groups;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Concurrent;
using Guppy.Extensions.Concurrent;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Frameable
    {
        #region Private Fields
        private NetPeer _peer;
        private NetIncomingMessage _im;
        #endregion

        #region Protected Fields

        #endregion

        #region Public Fields
        public UserCollection Users { get; private set; }
        public GroupCollection Groups { get; private set; }
        public IncomingMessageTypeDelegater MessagesTypes { get; private set; }
        public IList<NetConnection> Connections { get => _peer.Connections; }
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

            this.Users = provider.GetRequiredService<UserCollection>();
            this.Groups = provider.GetRequiredService<GroupCollection>();
            this.MessagesTypes = provider.GetRequiredService<IncomingMessageTypeDelegater>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.MessagesTypes.TryAdd(NetIncomingMessageType.Data, this.HandleData);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Users.Dispose();
            this.Groups.Dispose();
            this.MessagesTypes.Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.TryHandleIncomingMessages();
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Read all incoming messages. If there is an error in the process,
        /// skip the message and continue
        /// </summary>
        private void TryHandleIncomingMessages()
        {
#if DEBUG
            // Read any new incoming messages...
            while ((_im = _peer.ReadMessage()) != null)
                this.MessagesTypes.TryInvoke(this, _im.MessageType, _im);
#else
            try
            {
                // Read any new incoming messages...
                while ((_im = _peer.ReadMessage()) != null)
                    this.MessagesTypes.TryInvoke(this, _im.MessageType, _im);
            }
            catch(Exception e)
            {
                this.logger.LogError($"Error handling incoming message. Disconnecting connection & skipping message. ({e.GetType().Name}: {e.Message})");
                _im?.SenderConnection.Disconnect("Goodbye.");
                this.TryHandleIncomingMessages();
            }
#endif
        }
        #endregion

        protected internal abstract Type GroupType();

        #region MessageType Handlers
        private void HandleData(object sender, NetIncomingMessage im)
        {
            this.Groups.GetOrCreateById(im.ReadGuid()).Messages.Enqueue(im);
        }
        #endregion
    }
}
