using Guppy.Network.Enums;
using Guppy.Network.Providers;
using Guppy.Network.Security;
using Guppy.Network.Security.Services;
using Guppy.Settings.Providers;
using Guppy.Threading;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public abstract class Peer : IDisposable
    {
        private readonly IRoomProvider _rooms;
        private readonly INetMessengerProvider _messengers;
        private readonly IUserService _users;
        private readonly EventBasedNetListener _listener;
        private readonly NetManager _manager;
        private User? _currentUser;

        public IRoomProvider Rooms => _rooms;
        public readonly Room Room;

        public IUserService Users => _users;
        public User? CurrentUser
        {
            get => _currentUser;
            set => this.OnCurrentUserChanged.InvokeIf(value != _currentUser, this, ref _currentUser, value);
        }

        public event OnChangedEventDelegate<Peer, User?>? OnCurrentUserChanged;

        public Peer(
            IRoomProvider rooms,
            INetMessengerProvider messengers,
            IUserService users,
            ISettingProvider settings,
            EventBasedNetListener listener,
            NetManager manager,
            Bus bus)
        {
            _rooms = rooms;
            _messengers = messengers;
            _users = users;
            _listener = listener;
            _manager = manager;

            this.Room = _rooms.Get(0);
            this.Room.Messages.AttachBus(bus);

            this.OnCurrentUserChanged += this.HandleCurrentUserChanged;
            _listener.NetworkReceiveEvent += this.HandleNetworkReceiveEvent;

            settings.Get<HostType>().Value = HostType.Remote;
        }

        public virtual void Dispose()
        {
            this.OnCurrentUserChanged -= this.HandleCurrentUserChanged;
            _listener.NetworkReceiveEvent -= this.HandleNetworkReceiveEvent;

            _manager.Stop();
        }

        /// <summary>
        /// Receive all pending events. Call this in game update code
        /// In Manual mode it will call also socket Receive (which can be slow)
        /// </summary>
        public void PollEvents()
        {
            _manager.PollEvents();
        }

        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            NetIncomingMessage message = _messengers.ReadIncoming(reader);
            _rooms.ProcessIncoming(message);
        }

        private void HandleCurrentUserChanged(Peer sender, User? old, User? value)
        {
            if (old is not null)
            {
                old.IsCurrentUser = false;
            }

            if (value is not null)
            {
                value.IsCurrentUser = true;
            }
        }
    }
}
