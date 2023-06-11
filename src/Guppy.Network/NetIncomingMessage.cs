using Guppy.Common;
using Guppy.Network.Providers;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    internal sealed class NetIncomingMessage<T> : INetIncomingMessage<T>
        where T : notnull
    {
        private readonly NetScope _scope;
        private readonly NetMessageType<T> _type;
        private readonly INetSerializerProvider _serializers;
        private readonly INetSerializer<T> _serializer;
        private NetPeer? _peer;
        private T _body;
        private byte _channel;
        private DeliveryMethod _deliveryMethod;

        T INetIncomingMessage<T>.Body => _body;

        public byte Channel => _channel;

        public DeliveryMethod DeliveryMethod => _deliveryMethod;

        NetMessageType<T> INetIncomingMessage<T>.Type => _type;

        NetPeer? INetIncomingMessage.Peer => _peer!;

        object INetIncomingMessage.Body => _body;

        NetMessageType INetIncomingMessage.Type => _type;

        Type IMessage.Type { get; } = typeof(INetIncomingMessage<T>);

        internal NetIncomingMessage(
            NetMessageType<T> type,
            NetScope scope,
            INetSerializerProvider serializers)
        {
            _body = default!;
            _type = type;
            _scope = scope;
            _serializers = serializers;
            _serializer = _serializers.Get<T>();
        }

        public void Read(NetPeer? peer, NetDataReader reader, ref byte channel, ref DeliveryMethod deliveryMethod)
        {
            _peer = peer;
            _body = _serializer.Deserialize(reader);
            _channel = channel;
            _deliveryMethod = deliveryMethod;
        }

        public void Recycle()
        {
            _type.Recycle(this);
        }

        public INetIncomingMessage<T> Enqueue()
        {
            _scope.Bus.Enqueue(this);

            return this;
        }

        INetIncomingMessage INetIncomingMessage.Enqueue()
        {
            return this.Enqueue();
        }

        void IDisposable.Dispose()
        {
            this.Recycle();
        }
    }
}
