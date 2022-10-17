using Guppy.Common;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Identity.Services;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using Guppy.Resources;
using Guppy.Resources.Providers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public sealed class NetScope : Broker<INetMessage>, 
        ISubscriber<INetOutgoingMessage>,
        ISubscriber<NetIncomingMessage<UserAction>>, 
        IDisposable
    {
        private NetState _state;
        private readonly INetMessageProvider _messages;
        private readonly IUserProvider _users;

        internal byte id;

        public byte Id => this.id;
        public NetState State
        {
            get => _state;
            set => this.OnStateChanged!.InvokeIf(value != _state, this, ref _state, value);
        }

        public IUserService Users { get; }
        public ISetting<NetAuthorization> Authorization { get; }

        public readonly IBus Bus;

        public event OnChangedEventDelegate<NetScope, NetState>? OnStateChanged;

        public NetScope(
            ISettingProvider settings, 
            INetMessageProvider messages,
            IUserProvider users,
            IBus bus)
        {
            _state = NetState.Stopped;
            _messages = messages;
            _users = users;

            this.Users = new UserService();
            this.Authorization = settings.Get<NetAuthorization>();
            this.Bus = bus;

            this.Bus.Subscribe<INetOutgoingMessage>(this);
            this.Bus.Subscribe<NetIncomingMessage<UserAction>>(this);

            this.Users.OnUserJoined += this.HandleUserJoined;
            this.Users.OnUserLeft += this.HandleUserLeft;
        }

        public void Dispose()
        {
            this.Bus.Unsubscribe<INetOutgoingMessage>(this);
            this.Bus.Unsubscribe<NetIncomingMessage<UserAction>>(this);

            this.Users.OnUserJoined -= this.HandleUserJoined;
            this.Users.OnUserLeft -= this.HandleUserLeft;

            this.State = NetState.Disposed;
            this.Users.Dispose();
        }

        public void Start(byte id)
        {
            if (this.State != NetState.Stopped)
            {
                return;
            }

            this.id = id;
            this.State = NetState.Started;
        }

        public void Stop()
        {
            if (this.State != NetState.Started)
            {
                return;
            }

            this.State = NetState.Stopped;
        }

        public NetOutgoingMessage<TBody> Create<TBody>(in TBody body)
        {
            return _messages.Create(in body, this);
        }
        public NetOutgoingMessage<TBody> Create<TBody>(TBody body)
        {
            return _messages.Create(in body, this);
        }

        void ISubscriber<INetOutgoingMessage>.Process(in INetOutgoingMessage message)
        {
            message.Send();
        }

        void ISubscriber<NetIncomingMessage<UserAction>>.Process(in NetIncomingMessage<UserAction> message)
        {
            if (this.Authorization.Value != NetAuthorization.Slave)
            {
                return;
            }

            var user = _users.UpdateOrCreate(message.Body.Id, message.Body.Claims);

            switch (message.Body.Action)
            {
                case UserAction.Actions.UserJoined:
                    this.Users.Add(user);
                    break;
                case UserAction.Actions.UserLeft:
                    this.Users.Remove(user);
                    break;
            }
        }

        private void HandleUserJoined(IUserService sender, User user)
        {
            if(this.Authorization.Value != NetAuthorization.Master)
            {
                return;
            }

            this.Create(user.CreateAction(UserAction.Actions.UserJoined, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();
        }

        private void HandleUserLeft(IUserService sender, User user)
        {
            if (this.Authorization.Value != NetAuthorization.Master)
            {
                return;
            }

            this.Create(user.CreateAction(UserAction.Actions.UserLeft, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();
        }
    }
}
