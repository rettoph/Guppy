using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.EventArgs;
using Guppy.Network.Security.Lists;
using Guppy.Network.Services;
using LiteNetLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network
{
    /// <summary>
    /// A room is a targetable 
    /// </summary>
    public class Room : Entity
    {
        #region Private Fields
        private Boolean _isScopedLinked;
        private NetworkMessageService _messages;
        private Peer _peer;
        #endregion

        #region Public Properties
        public Byte Id { get; internal set; }
        public UserList Users { get; private set; }
        public PipeService Pipes { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _peer);

            this.Users = provider.GetService<UserList>();
            this.Pipes = provider.GetService<PipeService>((pipes, _, _) => pipes.room = this);

            this.Users.OnUserAdded += this.HandleUserAdded;
            this.Users.OnUserRemoved += this.HandleUserRemoved;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _peer = default;

            while(this.Users.Any())
            {
                this.Users.TryRemove(this.Users.First());
            }

            this.Users.OnUserAdded -= this.HandleUserAdded;
            this.Users.OnUserRemoved -= this.HandleUserRemoved;
            this.Users.TryRelease();

            this.TryUnlinkScope();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Attempt to link the room to the current scope.
        /// If a scope is already linked, this will fail,
        /// otherwise, the provider's MessageManager will
        /// be utilized for incoming messages.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Boolean TryLinkScope(ServiceProvider provider)
        {
            if(_isScopedLinked)
            {
                return false;
            }

            provider.Service(out _messages);

            _isScopedLinked = true;
            return true;
        }

        public Boolean TryUnlinkScope()
        {
            if (!_isScopedLinked)
            {
                return false;
            }

            _messages = default;

            _isScopedLinked = false;
            return true;
        }

        /// <summary>
        /// Process an incoming message
        /// </summary>
        /// <param name="message"></param>
        public void EnqueueIncoming(NetworkMessage message)
        {
            _messages.Enqueue(message.Data);
        }

        /// <summary>
        /// Send a message within the current room to a specific recipient
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TData>(TData data, NetPeer reciepient)
            where TData : class, IData
        {
            _peer.SendMessage(this, data, reciepient);
        }

        /// <summary>
        /// Send a message within the current room to a specific collection of recipients
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TData>(TData data, IEnumerable<NetPeer> reciepients)
            where TData : class, IData
        {
            _peer.SendMessage(this, data, reciepients);
        }

        /// <summary>
        /// Send a message within the current room to all joined peers
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TData>(TData data)
            where TData : class, IData
        {
            _peer.SendMessage(this, data, this.Users.NetPeers);
        }
        #endregion

        #region Frame Methods
        /// <summary>
        /// Update the internal room. This should be called manually, generally within
        /// a scene.
        /// </summary>
        /// <param name="gameTime"></param>
        public void TryUpdate(GameTime gameTime)
        {
            _messages.ProcessEnqueued();
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(UserList sender, UserEventArgs args)
        {
            args.User.AddToRoom(this);
        }

        private void HandleUserRemoved(UserList sender, UserEventArgs args)
        {
            args.User.RemoveFromRoom(this);
        }
        #endregion
    }
}
