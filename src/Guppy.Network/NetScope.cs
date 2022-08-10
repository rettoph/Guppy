using Guppy.Common;
using Guppy.Network.Enums;
using Guppy.Network.Providers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public sealed class NetScope : Broker<INetIncomingMessage>, IDisposable
    {
        private NetState _state;
        private readonly ConcurrentQueue<INetIncomingMessage> _incoming;
        private readonly ConcurrentQueue<INetOutgoingMessage> _outgoing;

        internal byte id;

        public byte Id => this.id;
        public NetState State
        {
            get => _state;
            set => this.OnStateChanged!.InvokeIf(value != _state, this, ref _state, value);
        }

        public event OnChangedEventDelegate<NetScope, NetState>? OnStateChanged;

        internal NetScope()
        {
            _incoming = new ConcurrentQueue<INetIncomingMessage>();
            _outgoing = new ConcurrentQueue<INetOutgoingMessage>();
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

        public void Dispose()
        {
            this.State = NetState.Disposed;
        }
    }
}
