using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Guppy.Network.Components;
using Guppy.Network.Constants;
using Guppy.Network;
using Guppy.Network.Enums;
using Guppy.Network.Providers;
using Guppy.Network.Services;
using Guppy.Providers;
using Guppy.Threading;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public sealed class NetScope : IDisposable
    {
        private NetState _state;
        private readonly IEntityService _entities;
        private readonly INetScopeProvider _scopes;
        private readonly DoubleDictionary<int, Type, ConcurrentQueue<NetOutgoingMessage>> _outgoing;
        private readonly Queue<NetIncomingMessage> _incoming;

        internal byte id;

        public byte Id => this.id;
        public NetState State
        {
            get => _state;
            set => this.OnStateChanged!.InvokeIf(value != _state, this, ref _state, value);
        }

        public readonly INetMessengerService Messengers;

        public Room? Room { get; private set; }

        public event OnChangedEventDelegate<NetScope, NetState>? OnStateChanged;

        public NetScope(INetScopeProvider scopes, IEntityService entities, INetMessengerService messengers, INetMessageProvider messages)
        {
            _scopes = scopes;
            _entities = entities;
            _incoming = new Queue<NetIncomingMessage>();
            _outgoing = messages.OrderBy(x => x.OutgoingPriority)
                 .ToDoubleDictionary(
                     keySelector1: x => x.OutgoingPriority,
                     keySelector2: x => x.Type,
                     elementSelector: x => new ConcurrentQueue<NetOutgoingMessage>());

            this.Messengers = messengers;
            this.State = NetState.Stopped;

            _scopes.TryAdd(this);
        }

        public void Dispose()
        {
            this.Stop();
            _scopes.TryRemove(this);
        }

        public void Start(byte id)
        {
            if(this.State != NetState.Stopped)
            {
                return;
            }

            this.id = id;
            this.State = NetState.Started;

            // Create the scopes system messenger...
            this.Room = _entities.Create<Room>();
        }

        public void Stop()
        {
            if (this.State != NetState.Started)
            {
                return;
            }

            this.State = NetState.Stopped;
        }

        internal void Enqueue(NetIncomingMessage message)
        {
            _incoming.Enqueue(message);
        }

        internal void Enqueue(NetOutgoingMessage message)
        {
            _outgoing[message.Factory.Type].Enqueue(message);
        }

        public void Clean()
        {
            // Read incoming messages
            while (_incoming.TryDequeue(out NetIncomingMessage? im))
            {
                if (this.Messengers.TryGet(im.MessengerId, out NetMessenger? messenger))
                {
                    messenger.Publish(im);
                }
                else
                {
                    // This should log gracefully
                    throw new Exception();
                }
            }

            // Send outgoing messages...
            int maximum = 100;
            int count = 0;

            foreach (ConcurrentQueue<NetOutgoingMessage> queue in _outgoing.Values)
            {
                while (count++ < maximum && queue.TryDequeue(out NetOutgoingMessage? message))
                {
                    message.Send();
                    message.Recycle();
                }
            }
        }
    }
}
