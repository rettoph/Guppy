using Guppy.Messaging;
using Guppy.Network.Providers;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Network
{
    internal sealed class NetIncomingMessage<T> : INetIncomingMessage<T>
        where T : notnull
    {
        private readonly IPeer _peer;
        private readonly INetSerializerProvider _serializers;
        private readonly INetSerializer<T> _serializer;

        public T Body { get; private set; }

        public byte Channel { get; private set; }

        public DeliveryMethod DeliveryMethod { get; private set; }

        public NetMessageType<T> Type { get; private set; }

        public INetGroup Group { get; private set; }

        object INetIncomingMessage.Body => this.Body;

        NetMessageType INetIncomingMessage.Type => this.Type;

        Type IMessage.Type { get; } = typeof(INetIncomingMessage<T>);

        internal NetIncomingMessage(
            IPeer peer,
            INetSerializerProvider serializers,
            NetMessageType<T> type)
        {
            _peer = peer;
            _serializers = serializers;
            _serializer = _serializers.Get<T>();

            this.Body = default!;
            this.Group = default!;
            this.Type = type;
        }

        public void Read(NetDataReader reader, ref byte channel, ref DeliveryMethod deliveryMethod)
        {
            byte groupId = reader.GetByte();
            this.Group = _peer.Groups.GetById(groupId);
            this.Body = _serializer.Deserialize(reader);
            this.Channel = channel;
            this.DeliveryMethod = deliveryMethod;
        }

        public void Recycle()
        {
            this.Type.Recycle(this);
        }

        public INetIncomingMessage<T> Enqueue()
        {
            this.Group.Scope.Enqueue(this);

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
