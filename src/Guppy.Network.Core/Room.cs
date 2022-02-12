using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.EventArgs;
using Guppy.Network.Security.Lists;
using Guppy.Network.Services;
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
        private MessageBus _bus;
        private Peer _peer;
        private NetworkProvider _network;
        #endregion

        #region Public Properties
        public new Byte Id { get; internal set; }
        public UserList Users { get; private set; }
        public PipeService Pipes { get; private set; }
        public MessageBus Bus => _bus;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _peer);
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

            // Determin which message configurations are valid within the current scope...
            IEnumerable<NetworkMessageConfiguration> configurations = _network.MessageConfigurations.Where(mc => mc.Filter(provider, mc));

            // Generate a lookup table of valid message configuration bus queues and their types...
            Dictionary<Int32, Type[]> messageBusQueus = configurations.GroupBy(configuration => configuration.MessageBusQueue)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => g.Select(configuration => configuration.Type).ToArray());


            // Ensure that all required message bus queus have been registered...
            provider.Service(out _bus);

            foreach ((Int32 queue, Type[] types) in messageBusQueus)
            {
                _bus.GetQueue(queue).RegisterTypes(types);
            }

            // Register all message processors into the bus
            foreach (NetworkMessageConfiguration configuration in configurations)
            {
                configuration.TryRegisterProcessor(provider, _bus);
            }

            _isScopedLinked = true;
            return true;
        }

        public Boolean TryUnbindToScope()
        {
            if (!_isScopedLinked)
            {
                return false;
            }

            _bus = default;

            _isScopedLinked = false;
            return true;
        }

        /// <summary>
        /// Process an incoming message
        /// </summary>
        /// <param name="message"></param>
        public void TryEnqueueIncomingMessage(NetworkMessage message)
        {
            _bus.Enqueue(message.Data);
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
