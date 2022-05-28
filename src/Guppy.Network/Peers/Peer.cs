using Guppy.Network.Enums;
using Guppy.Network.Providers;
using Guppy.Network.Security;
using Guppy.Network.Security.Providers;
using Guppy.Providers;
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
        private readonly INetMessageProvider _messengers;
        private readonly IUserProvider _users;
        private readonly EventBasedNetListener _listener;
        private readonly NetManager _manager;
        private User? _currentUser;

        public readonly NetScope Scope;
        public readonly INetScopeProvider Scopes;
        public IUserProvider Users => _users;
        public User? CurrentUser
        {
            get => _currentUser;
            set => this.OnCurrentUserChanged!.InvokeIf(value != _currentUser, this, ref _currentUser, value);
        }

        public event OnChangedEventDelegate<Peer, User?>? OnCurrentUserChanged;

        public Peer(
            INetScopeProvider scopes,
            INetMessageProvider messengers,
            IUserProvider users,
            ISettingProvider settings,
            EventBasedNetListener listener,
            NetManager manager,
            NetScope scope)
        {
            _messengers = messengers;
            _users = users;
            _listener = listener;
            _manager = manager;

            this.Scope = scope;
            this.Scopes = scopes;

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

        protected virtual void Start()
        {
            this.Scope.Start(0);
        }

        /// <summary>
        /// Receive all pending events. Call this in game update code
        /// In Manual mode it will call also socket Receive (which can be slow)
        /// </summary>
        public void PollEvents()
        {
            _manager.PollEvents();
            this.Scope.Clean();
        }

        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            NetIncomingMessage message = _messengers.CreateIncoming(peer, reader);
            
            if(this.Scopes.TryGet(message.ScopeId, out NetScope? scope))
            {
                scope.Enqueue(message);
            }
            else
            {
                // When the room doesnt exist i think we should cache the messages for later
                throw new Exception();
            }
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
