using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using Guppy.DependencyInjection;
using Guppy.Network.Collections;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Messageable
    {
        #region Private Fields
        private NetPeer _peer;
        private NetIncomingMessage _im;
        #endregion

        #region Public Attributes
        /// <summary>
        /// List of all connected users.
        /// </summary>
        public UserCollection Users { get; private set; }

        public User CurrentUser { get; protected set; }
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

            _peer = this.GetPeer(provider);

            this.provider = provider;

            this.MessageTypeDelegates = new Dictionary<NetIncomingMessageType, MessageTypeDelegate>(((NetIncomingMessageType[])Enum.GetValues(typeof(NetIncomingMessageType))).ToDictionary(
                keySelector: mt => mt,
                elementSelector: mt => default(MessageTypeDelegate)));

            this.Groups = provider.GetService<GroupCollection>();
            this.Users = provider.GetService<UserCollection>();
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.MessageTypeDelegates[NetIncomingMessageType.Data] += this.HandleDataMessageType;
        }

        protected override void Release()
        {
            base.Release();

            this.MessageTypeDelegates[NetIncomingMessageType.Data] -= this.HandleDataMessageType;
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

        #region MessageType Handlers
        private void HandleDataMessageType(NetIncomingMessage im)
        {
            switch ((MessageTarget)im.ReadByte())
            {
                case MessageTarget.Group:
                    this.Groups.GetOrCreateById(im.ReadGuid()).IncomingMessages.Enqueue(im);
                    break;
                case MessageTarget.Peer:
                    im.ReadGuid();
                    this.IncomingMessages.Enqueue(im);
                    break;
            }
        }

        #endregion

        #region Messageable Implementation
        protected override MessageTarget TargetType()
            => MessageTarget.Peer;
        #endregion
    }
}
