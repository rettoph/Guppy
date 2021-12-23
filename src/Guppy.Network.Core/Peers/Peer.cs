﻿using Minnow.General;
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
using Guppy.EntityComponent;
using System.Threading;
using Guppy.Utilities.Threading;
using System.CommandLine.Binding;

namespace Guppy.Network
{
    /// <summary>
    /// The primary peer class, used as a wrapper for all client/server connections.
    /// </summary>
    public class Peer : Service
    {
        /// <summary>
        /// The default RoomId used for inner Peer communication.
        /// Try to avoid using this room id unless you know what
        /// you are doing.
        /// </summary>
        public const Byte RoomId = Byte.MaxValue;

        #region Private Fields
        private User _currentUser;
        private RoomService _rooms;
        private Room _room;
        private CancellationTokenSource _cancelation;
        private Task _loop;
        #endregion

        #region Protected Properties
        protected EventBasedNetListener listener { get; private set; }
        protected NetManager manager { get; private set; }
        protected NetworkProvider network { get; private set; }
        protected CommandService commands { get; private set; }
        protected Room room => _room;
        #endregion

        #region Public Properties
        public UserService Users { get; private set; }

        public User CurrentUser
        {
            get => _currentUser;
            set => this.OnCurrentUserChanged.InvokeIf(value != _currentUser, this, ref _currentUser, value);
        }

        public RoomService Rooms => _rooms;
        #endregion

        #region Events
        public OnChangedEventDelegate<Peer, User> OnCurrentUserChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreCreate(ServiceProvider provider)
        {
            base.Create(provider);

            provider.Service(out _rooms);

            this.listener = provider.GetService<EventBasedNetListener>();
            this.manager = provider.GetService<NetManager>();
            this.network = provider.GetService<NetworkProvider>();
            this.commands = provider.GetService<CommandService>();

            this.Users = provider.GetService<UserService>();

            provider.Settings.Set(HostType.Remote);

            this.commands.Get<Commands.Network.Users>().Handler = CommandHandler.Create<Int32?, IConsole>(this.HandleNetworkUserCommand);
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

            _room = _rooms.GetById(RoomId);
            _room.TryLinkScope(provider);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.listener.NetworkReceiveEvent -= this.HandleNetworkReceiveEvent;

            _cancelation?.Cancel();
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
        protected async Task TryStartAsync(Int32 period)
        {
            _cancelation = new CancellationTokenSource();

            _loop = TaskHelper.CreateLoop(this.Update, period, _cancelation.Token);

            await _loop;
        }
        #endregion

        #region SendMessage Methods
        public void SendMessage<TData>(Room room, TData data, NetPeer recipient)
            where TData : class, IData
        {
            this.network.SendMessage(room, data, recipient);
        }
        public void SendMessage<TData>(Room room, TData data, IEnumerable<NetPeer> recipients)
            where TData : class, IData
        {
            this.network.SendMessage(room, data, recipients); 
        }
        protected void SendMessage<TData>(TData data, NetPeer recipient)
            where TData : class, IData
        {
            this.SendMessage(_room, data, recipient);
        }
        protected void SendMessage<TData>(TData data, IEnumerable<NetPeer> recipients)
            where TData : class, IData
        {
            this.SendMessage(_room, data, recipients);
        }
        #endregion

        #region Frame Methods
        protected virtual void Update(GameTime gameTime)
        {
            this.manager.PollEvents();
            this.room.TryUpdate(gameTime);
        }
        #endregion

        #region Event Methods
        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            NetworkMessage message = this.network.ReadMessage(reader);

            Console.WriteLine($"Recived Message => {message.Data.GetType().Name}");
            _rooms.EnqueueIncoming(message);
        }
        #endregion

        #region Command Handlers
        private void HandleNetworkUserCommand(Int32? id, IConsole console)
        {
            if(id.HasValue)
            { // Print user specific data...
                if(this.Users.TryGetById(id.Value, out User user))
                {
                    console.Out.WriteLine($"{nameof(User.Id)}: {user.Id}, {nameof(User.CreatedAt)}: {user.CreatedAt:HH:mm:ss}, {nameof(User.UpdatedAt)}: {user.UpdatedAt:HH:mm:ss}");

                    console.Out.WriteLine("------------------------------------------------");

                    console.Out.WriteLine("Claim(s)");
                    foreach (Claim claim in user.Claims)
                    {
                        console.Out.WriteLine($"  {claim.Key}, {claim.Value}, {claim.Type}, {claim.CreatedAt:HH:mm:ss}");
                    }

                    console.Out.WriteLine("------------------------------------------------");
                    console.Out.WriteLine("Room(s)");
                    foreach(Room room in user.Rooms)
                    {
                        console.Out.WriteLine($"  {nameof(Room.Id)}: {room.Id}");
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
                    console.Out.WriteLine($"{nameof(User.Id)}: {user.Id}, Claim(s): {user.Claims.Count()}, Room(s): {user.Rooms.Count()}");
                }
            }
        }
        #endregion
    }
}
