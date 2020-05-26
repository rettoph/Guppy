using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using Guppy.DependencyInjection;
using Guppy.Network.Collections;
using Guppy.Network.Groups;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Messageable
    {
        #region Private Fields
        private NetPeer _peer;
        private NetIncomingMessage _im;
        #endregion

        #region Protected Attribtes
        protected ServiceProvider provider { get; private set; }
        #endregion

        #region Public Attributes
        /// <summary>
        /// The peer's update interval in milliseconds.
        /// </summary>
        public Int32 Interval { get; set; } = 32;
        public Dictionary<NetIncomingMessageType, MessageTypeDelegate> MessageTypeDelegates { get; private set; }
        public GroupCollection Groups { get; private set; }
        #endregion

        #region Delegates
        public delegate void MessageTypeDelegate(NetIncomingMessage im);
        #endregion

        #region Constructor
        internal Peer()
        {

        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.provider = provider;

            this.MessageTypeDelegates = new Dictionary<NetIncomingMessageType, MessageTypeDelegate>(((NetIncomingMessageType[])Enum.GetValues(typeof(NetIncomingMessageType))).ToDictionary(
                keySelector: mt => mt,
                elementSelector: mt => default(MessageTypeDelegate)));

            _peer = this.GetPeer(provider);

            this.Groups = provider.GetService<GroupCollection>();
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.MessageTypeDelegates = null;
        }
        #endregion

        #region Helper Methods
        protected abstract NetPeer GetPeer(ServiceProvider provider);
        internal abstract Group GroupFactory();
        #endregion

        #region Frame Methods
        protected override void Start(bool draw)
        {
            _peer.Start();

            base.Start(draw);
        }

        protected override void Update(GameTime gameTime)
        {
            while ((_im = _peer.ReadMessage()) != null)
                this.MessageTypeDelegates[_im.MessageType]?.Invoke(_im);

            base.Update(gameTime);
        }
        #endregion
    }
}
