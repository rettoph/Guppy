using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.EventArgs;
using Guppy.Network.Security.Lists;
using Guppy.Network.Services;
using Guppy.Network.Utilities;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
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
        private NetworkProvider _network;

        private RoomMessageManager _messages;
        #endregion

        #region Public Properties
        public new Byte Id { get; internal set; }
        public UserList Users { get; private set; }
        public PipeService Pipes { get; private set; }

        public RoomMessageManager Messages => _messages;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _network);

            this.Users = provider.GetService<UserList>();
            this.Pipes = provider.GetService<PipeService>((pipes, _, _) => pipes.room = this);

            this.Users.OnEvent += this.HandleUserListEvent;
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            while (this.Users.Any())
            {
                this.Users.TryRemove(this.Users.First());
            }

            this.Users.OnEvent -= this.HandleUserListEvent;

            this.TryUnbindToScope();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Attempt to link the room to the current scope.
        /// If a scope is already linked, this will fail,
        /// otherwise, the provider's scoped MessageBus will
        /// be utilized for incoming messages.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Boolean TryBindToScope(ServiceProvider provider)
        {
            if (_isScopedLinked)
            {
                return false;
            }

            _messages = new RoomMessageManager(
                this, 
                _network,
                provider);

            _isScopedLinked = true;
            return true;
        }

        public Boolean TryUnbindToScope()
        {
            if (!_isScopedLinked)
            {
                return false;
            }

            _messages.Dispose();

            _isScopedLinked = false;
            return true;
        }
        #endregion

        #region Event Handlers
        private void HandleUserListEvent(UserList sender, UserListEventArgs args)
        {
            switch (args.Action)
            {
                case Security.Enums.UserListAction.Added:
                    args.User.AddToRoom(this);
                    break;
                case Security.Enums.UserListAction.Removed:
                    args.User.RemoveFromRoom(this);
                    break;
            }
        }
        #endregion
    }
}
