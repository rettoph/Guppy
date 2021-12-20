using Minnow.General;
using Guppy.Network.Messages;
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
using Guppy.CommandLine.Services;
using System.CommandLine.Invocation;
using System.CommandLine;
using System.CommandLine.IO;
using System.Linq;
using Guppy.Network.Security.Structs;
using Guppy.Threading.Utilities;

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
        private ThreadQueue _threadQueue;
        #endregion

        #region Protected Properties
        protected EventBasedNetListener listener { get; private set; }
        protected NetManager manager { get; private set; }
        protected NetworkProvider network { get; private set; }
        protected CommandService commands { get; private set; }
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
            this.commands = provider.GetService<CommandService>();

            this.Users = provider.GetService<UserService>();

            provider.Settings.Set(HostType.Local);

            this.commands.Get("network users").Handler = CommandHandler.Create<Int32?, IConsole>(this.HandleNetworkUserCommand);
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
        public void SendMessage<TData>(TData data, NetPeer recipient)
            where TData : class, IData
        {
            NetDataWriter om = _writerFactory.GetInstance();
            this.network.SendMessage(om, data, recipient);
            _writerFactory.TryReturnToPool(om);
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
            NetworkMessage message = this.network.ReadMessage(reader);
            _room.ProcessIncoming(message);
        }
        #endregion

        #region Command Handlers
        private void HandleNetworkUserCommand(Int32? id, IConsole console)
        {
            if(id.HasValue)
            { // Print user specific data...
                if(this.Users.TryGetById(id.Value, out User user))
                {
                    console.Out.WriteLine($"{nameof(User.Id)}: {user.Id}");

                    foreach(Claim claim in user.Claims)
                    {
                        console.Out.WriteLine($"{claim.Key}, {claim.Value}, {claim.Type}");
                    }
                }
                else
                {
                    console.Error.WriteLine($"Unable to find {nameof(User)}: {id.Value}");
                }
            }
            else
            { // Print all user overview...
                foreach(User user in this.Users)
                {
                    console.Out.WriteLine($"{nameof(User.Id)}: {user.Id}, Claims: {user.Claims.Count()}");
                }
            }
        }
        #endregion
    }
}
