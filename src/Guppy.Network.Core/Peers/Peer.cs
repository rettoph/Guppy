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
using Guppy.EntityComponent;
using System.Threading;
using System.CommandLine.Binding;
using Guppy.Network.Messages.Commands;
using Guppy.Threading.Interfaces;
using log4net;
using Guppy.Threading.Helpers;

namespace Guppy.Network
{
    /// <summary>
    /// The primary peer class, used as a wrapper for all client/server connections.
    /// </summary>
    public class Peer : Service, IDataProcessor<GuppyNetworkUsersCommand>
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
        private ILog _log;
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
            provider.Service(out _log);

            this.listener = provider.GetService<EventBasedNetListener>();
            this.manager = provider.GetService<NetManager>();
            this.network = provider.GetService<NetworkProvider>();
            this.commands = provider.GetService<CommandService>();

            this.Users = provider.GetService<UserService>();

            provider.Settings.Set(HostType.Remote);

            this.commands.RegisterProcessor<GuppyNetworkUsersCommand>(this);
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
            _room.TryBindToScope(provider);
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

            _rooms = default;
            _log = default;

            this.listener = default;
            this.manager = default;
            this.network = default;

            this.Users = default;
        }
        #endregion

        #region Start Methods
        protected virtual async Task TryStart(Int32 period)
        {
            _cancelation = new CancellationTokenSource();

            _loop = TaskHelper.CreateLoop(this.Update, period, _cancelation.Token);

            await _loop;
        }

        public virtual async Task TryStop()
        {
            if(_cancelation is not null)
            {
                _cancelation.Cancel();

                await _loop;

                this.manager.Stop();

                _cancelation = null;
                _log = null;
            }
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

            _room.Update();
        }
        #endregion

        #region Event Methods
        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            NetworkMessage message = this.network.ReadMessage(reader);
            _rooms.ProcessIncomingMessage(message);
        }
        #endregion

        #region Command Handlers
        public void Process(GuppyNetworkUsersCommand data)
        {
            if (data.Id.HasValue)
            { // Print user specific data...
                if (this.Users.TryGetById(data.Id.Value, out User user))
                {
                    _log.Info($"{nameof(User.Id)}: {user.Id}, {nameof(User.CreatedAt)}: {user.CreatedAt:HH:mm:ss}, {nameof(User.UpdatedAt)}: {user.UpdatedAt:HH:mm:ss}");

                    _log.Info("------------------------------------------------");

                    _log.Info("Claim(s)");
                    foreach (Claim claim in user.Claims)
                    {
                        _log.Info($"  {claim.Key}, {claim.Value}, {claim.Type}, {claim.CreatedAt:HH:mm:ss}");
                    }

                    _log.Info("------------------------------------------------");
                    _log.Info("Room(s)");
                    foreach (Room room in user.Rooms)
                    {
                        _log.Info($"  {nameof(Room.Id)}: {room.Id}");
                    }
                }
                else
                {
                    _log.Error($"Unable to find {nameof(User)}: {data.Id.Value}");
                }
            }
            else
            { // Print all user overview...
                foreach (User user in this.Users)
                {
                    _log.Info($"{nameof(User.Id)}: {user.Id}, Claim(s): {user.Claims.Count()}, Room(s): {user.Rooms.Count()}");
                }
            }
        }
        #endregion
    }
}
