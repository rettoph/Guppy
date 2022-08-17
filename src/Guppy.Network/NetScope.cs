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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public sealed class NetScope : Broker<INetIncomingMessage>, ISubscriber<NetIncomingMessage<UserAction>>, IDisposable
    {
        private NetState _state;
        private readonly ConcurrentQueue<INetIncomingMessage> _incoming;
        private readonly ConcurrentQueue<INetOutgoingMessage> _outgoing;
        private readonly INetMessageProvider _messages;
        private readonly IUserProvider _users;

        internal byte id;

        public byte Id => this.id;
        public NetState State
        {
            get => _state;
            set => this.OnStateChanged!.InvokeIf(value != _state, this, ref _state, value);
        }

        public IUserService Users { get; set; }
        public ISetting<NetAuthorization> Authorization { get; }

        public event OnChangedEventDelegate<NetScope, NetState>? OnStateChanged;

        internal NetScope(ISetting<NetAuthorization> authorization, INetMessageProvider messages, IUserProvider users)
        {
            _state = NetState.Stopped;
            _incoming = new ConcurrentQueue<INetIncomingMessage>();
            _outgoing = new ConcurrentQueue<INetOutgoingMessage>();
            _messages = messages;
            _users = users;

            this.Users = new UserService();
            this.Authorization = authorization;

            this.Subscribe(this);

            this.Users.OnUserJoined += this.HandleUserJoined;
            this.Users.OnUserLeft += this.HandleUserLeft;
        }

        public void Dispose()
        {
            this.Unsubscribe(this);

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

        public void Enqueue(INetIncomingMessage message)
        {
            _incoming.Enqueue(message);
        }

        public void Enqueue(INetOutgoingMessage message)
        {
            _outgoing.Enqueue(message);
        }

        public void Flush()
        {
            while (_incoming.TryDequeue(out INetIncomingMessage? im))
            {
                this.Publish(im);
                im.Recycle();
            }

            while(_outgoing.TryDequeue(out INetOutgoingMessage? om))
            {
                om.Send();
                om.Recycle();
            }
        }

        public NetOutgoingMessage<THeader> Create<THeader>(in THeader header)
        {
            return _messages.Create(in header, this);
        }
        public NetOutgoingMessage<THeader> Create<THeader>(THeader header)
        {
            return _messages.Create(in header, this);
        }

        void ISubscriber<NetIncomingMessage<UserAction>>.Process(in NetIncomingMessage<UserAction> message)
        {
            if (this.Authorization.Value != NetAuthorization.Slave)
            {
                return;
            }

            var user = _users.UpdateOrCreate(message.Header.Id, message.Header.Claims);

            switch (message.Header.Action)
            {
                case UserAction.Actions.UserJoined:
                    this.Users.Add(user);
                    break;
                case UserAction.Actions.UserLeft:
                    this.Users.Remove(user);
                    break;
            }
        }

        private void HandleUserJoined(IUserService sender, User users)
        {
            if(this.Authorization.Value != NetAuthorization.Master)
            {
                return;
            }

            this.Create(users.CreateAction(UserAction.Actions.UserJoined, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();
        }

        private void HandleUserLeft(IUserService sender, User users)
        {
            if (this.Authorization.Value != NetAuthorization.Master)
            {
                return;
            }

            this.Create(users.CreateAction(UserAction.Actions.UserLeft, ClaimAccessibility.Public))
                .AddRecipients(this.Users.Peers)
                .Enqueue();
        }
    }
}
