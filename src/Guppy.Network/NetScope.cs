using Guppy.Common;
using Guppy.Common.Collections;
using Guppy.Common.Implementations;
using Guppy.Network.Definitions;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Factories;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Identity.Services;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using Guppy.Resources;
using Guppy.Resources.Providers;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Guppy.Common.ThrowIf;
using static System.Formats.Asn1.AsnWriter;

namespace Guppy.Network
{
    public sealed class NetScope : Broker, 
        ISubscriber<INetOutgoingMessage>,
        ISubscriber<INetIncomingMessage<UserAction>>, 
        IDisposable
    {
        private NetState _state;
        private readonly IUserProvider _users;
        private readonly INetSerializerProvider _serializers;
        private readonly DoubleDictionary<INetId, System.Type, NetMessageType> _messages;
        private readonly INetOutgoingMessageFactory _factory;

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
            IUserProvider users,
            IBus bus,
            INetSerializerProvider serializers,
            IFiltered<INetOutgoingMessageFactory> factories,
            IEnumerable<NetMessageTypeDefinition> definitions)
        {
            _state = NetState.Stopped;
            _serializers = serializers;
            _users = users;
            _factory = factories.Instance;

            _messages = new DoubleDictionary<INetId, System.Type, NetMessageType>(definitions.Count());

            byte id = 0;
            foreach (NetMessageTypeDefinition definition in definitions)
            {
                var type = definition.BuildType(NetId.Create(id), serializers, this);
                id += 1;

                _messages.TryAdd(type.Id, type.Body, type);
            }

            this.Users = new UserService();
            this.Authorization = settings.Get<NetAuthorization>();
            this.Bus = bus;

            this.Bus.Subscribe<INetOutgoingMessage>(this);
            this.Bus.Subscribe<INetIncomingMessage<UserAction>>(this);

            this.Users.OnUserJoined += this.HandleUserJoined;
            this.Users.OnUserLeft += this.HandleUserLeft;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Bus.Unsubscribe<INetOutgoingMessage>(this);
            this.Bus.Unsubscribe<INetIncomingMessage<UserAction>>(this);

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

            _factory.Initialize(_messages);

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

        public INetIncomingMessage Read(NetPeer? peer, NetDataReader reader)
        {
            var id = NetId.Byte.Read(reader);
            var message = _messages[id].CreateIncoming();
            message.Read(peer, reader);

            return message;
        }

        public INetOutgoingMessage<T> Create<T>(in T body)
            where T : notnull
        {
            return _factory.Create(in body);
        }

        void ISubscriber<INetOutgoingMessage>.Process(in INetOutgoingMessage message)
        {
            message.Send();
        }

        void ISubscriber<INetIncomingMessage<UserAction>>.Process(in INetIncomingMessage<UserAction> message)
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
