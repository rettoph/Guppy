using Guppy.EntityComponent;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using Guppy.Network.Services;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components
{
    public sealed class NetMessenger : Broker, IComponent
    {
        private ushort _id;
        private NetState _state;
        private readonly INetMessengerService _messengers;
        private readonly INetMessageProvider _messages;

        public readonly NetScope Scope;

        public ushort Id => _id;
        public NetState State
        {
            get => _state;
            set => this.OnStateChanged!.InvokeIf(value != _state, this, ref _state, value);
        }

        public INetTarget? Target { get; private set; }

        public event OnChangedEventDelegate<NetMessenger, NetState>? OnStateChanged;

        public NetMessenger(NetScope scope, INetMessengerService messengers, INetMessageProvider messages)
        {
            this.Scope = scope;

            _messengers = messengers;
            _messages = messages;
        }

        public void Initialize(IEntity entity)
        {
            this.Target = (INetTarget)entity;

            _messengers.Add(this);
        }

        public void Uninitilaize()
        {
            _messengers.Remove(this);
        }

        public void Dispose()
        {
            
        }

        public void Start(ushort id)
        {
            if (this.State != NetState.Stopped)
            {
                return;
            }

            _id = id;
            this.State = NetState.Started;
        }

        public void Stop()
        {
            if(this.State != NetState.Started)
            {
                return;
            }

            this.State = NetState.Stopped;
        }

        public NetOutgoingMessage<T> Create<T>(in T message)
        {
            return _messages.CreateOutgoing<T>(this, message);
        }
    }
}
