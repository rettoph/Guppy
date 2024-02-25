using Guppy.Messaging;
using Guppy.Network.Providers;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Network
{
    internal sealed class NetIncomingMessage<T> : INetIncomingMessage<T>
        where T : notnull
    {
        private readonly INetScope _scope;
        private readonly NetMessageType<T> _type;
        private readonly INetSerializerProvider _serializers;
        private readonly INetSerializer<T> _serializer;

        public T Body { get; private set; }

        public byte Channel { get; private set; }

        public DeliveryMethod DeliveryMethod { get; private set; }

        public NetMessageType<T> Type { get; private set; }

        public NetPeer? Peer { get; private set; }

        object INetIncomingMessage.Body => this.Body;

        NetMessageType INetIncomingMessage.Type => this.Type;

        Type IMessage.Type { get; } = typeof(INetIncomingMessage<T>);

        internal NetIncomingMessage(
            NetMessageType<T> type,
            INetScope scope,
            INetSerializerProvider serializers)
        {
            _type = type;
            _scope = scope;
            _serializers = serializers;
            _serializer = _serializers.Get<T>();

            this.Body = default!;
        }

        public void Read(NetPeer? peer, NetDataReader reader, ref byte channel, ref DeliveryMethod deliveryMethod)
        {
            this.Peer = peer;
            this.Body = _serializer.Deserialize(reader);
            this.Channel = channel;
            this.DeliveryMethod = deliveryMethod;
        }

        public void Recycle()
        {
            _type.Recycle(this);
        }

        public INetIncomingMessage<T> Enqueue()
        {
            _scope.Enqueue(this);

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
