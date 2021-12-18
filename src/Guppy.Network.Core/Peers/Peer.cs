using Minnow.General;
using Guppy.Network.Dtos;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Services;
using Guppy.Network.Services;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.Network
{
    /// <summary>
    /// The primary peer class, used as a wrapper for all client/server connections.
    /// </summary>
    public class Peer : Asyncable
    {
        #region Classes
        private class NetDataWriterFactory : Factory<NetDataWriter>
        {
            public NetDataWriterFactory() : base(() => new NetDataWriter(), 100)
            {
            }

            public override bool TryReturnToPool(NetDataWriter instance)
            {
                instance.Reset();
                return base.TryReturnToPool(instance);
            }
        }
        #endregion

        #region Private Fields
        private NetDataWriterFactory _writerFactory;
        private User _currentUser;
        private Room _room;
        #endregion

        #region Protected Properties
        protected EventBasedNetListener listener { get; private set; }
        protected NetManager manager { get; private set; }
        protected NetworkProvider network { get; private set; }
        #endregion

        #region Public Properties
        public UserService Users { get; private set; }

        public User CurrentUser
        {
            get => _currentUser;
            set => this.OnCurrentUserChanged.InvokeIf(value != _currentUser, this, ref _currentUser, value);
        }
        #endregion

        #region Events
        public OnChangedEventDelegate<Peer, User> OnCurrentUserChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreCreate(ServiceProvider provider)
        {
            base.Create(provider);

            _writerFactory = new NetDataWriterFactory();

            this.listener = provider.GetService<EventBasedNetListener>();
            this.manager = provider.GetService<NetManager>();
            this.network = provider.GetService<NetworkProvider>();

            this.Users = provider.GetService<UserService>();

            provider.Settings.Set(HostType.Local);
        }

        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.listener.NetworkReceiveEvent += this.HandleNetworkReceiveEvent;
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _room = provider.GetService<RoomService>().GetPeerRoom(provider);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.listener.NetworkReceiveEvent -= this.HandleNetworkReceiveEvent;
        }

        protected override void PostDispose()
        {
            base.Dispose();

            this.listener = default;
            this.manager = default;
            this.network = default;

            this.Users = default;
        }
        #endregion

        #region Start Methods
        /// <summary>
        /// Start logic thread and listening on available port
        /// </summary>
        public override Task TryStartAsync(bool draw = false, int period = 16)
        {
            this.manager.Start();

            return base.TryStartAsync(draw, period);
        }


        /// <summary>
        /// Start logic thread and listening on selected port
        /// </summary>
        /// <param name="port">port to listen</param>
        public Task TryStartAsync(Int32 port, bool draw = false, int period = 16)
        {
            this.manager.Start(port);

            return base.TryStartAsync(draw, period);
        }
        #endregion

        #region SendMessage Methods
        public void SendMessage(String messageName, IData data, NetPeer recipient)
        {
            this.UsingDataWriter(writer =>
            {
                this.network.SendMessage(writer, messageName, data, recipient);
            });
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.manager.PollEvents();
        }
        #endregion

        #region DataWriter Methods
        public void UsingDataWriter(Action<NetDataWriter> writer)
        {
            NetDataWriter om = _writerFactory.GetInstance();
            writer(om);
            _writerFactory.TryReturnToPool(om);
        }
        #endregion

        #region Event Methods
        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            Message message = this.network.ReadMessage(reader);
            _room.ProcessIncoming(message);
        }
        #endregion
    }
}
