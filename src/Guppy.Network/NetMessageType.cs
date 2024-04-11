using Guppy.Common.Collections;
using Guppy.Network.Services;
using LiteNetLib;

namespace Guppy.Network
{
    public abstract class NetMessageType
    {
        public readonly byte Id;
        public readonly Type Body;
        public readonly DeliveryMethod DefaultDeliveryMethod;
        public readonly byte DefaultOutgoingChannel;

        protected NetMessageType(byte id, Type header, DeliveryMethod defaultDeliveryMethod, byte defaultOutgoingChannel)
        {
            this.Id = id;
            this.Body = header;
            this.DefaultDeliveryMethod = defaultDeliveryMethod;
            this.DefaultOutgoingChannel = defaultOutgoingChannel;
        }

        public abstract INetIncomingMessage CreateIncoming();
    }

    public sealed class NetMessageType<T> : NetMessageType
        where T : notnull
    {
        private readonly IPeer _peer;
        private readonly INetSerializerService _serializers;
        private readonly Factory<NetIncomingMessage<T>> _incomingFactory;
        private readonly Factory<NetOutgoingMessage<T>> _outgoingFactory;

        public NetMessageType(
            byte id,
            Type body,
            DeliveryMethod defaultDeliveryMethod,
            byte defaultOutgoingChannel,
            IPeer peer,
            INetSerializerService serializers) : base(id, body, defaultDeliveryMethod, defaultOutgoingChannel)
        {
            _peer = peer;
            _serializers = serializers;
            _incomingFactory = new Factory<NetIncomingMessage<T>>(this.IncomingFactoryMethod);
            _outgoingFactory = new Factory<NetOutgoingMessage<T>>(this.OutgoingFactoryMethod);
        }

        private NetIncomingMessage<T> IncomingFactoryMethod()
        {
            return new NetIncomingMessage<T>(_peer, _serializers, this);
        }

        private NetOutgoingMessage<T> OutgoingFactoryMethod()
        {
            return new NetOutgoingMessage<T>(_peer, _serializers, this);
        }

        public override INetIncomingMessage<T> CreateIncoming()
        {
            return _incomingFactory.BuildInstance();
        }

        public INetOutgoingMessage<T> CreateOutgoing()
        {
            return _outgoingFactory.BuildInstance();
        }

        internal void Recycle(NetIncomingMessage<T> message)
        {
            _incomingFactory.TryReturn(ref message);
        }

        internal void Recycle(NetOutgoingMessage<T> message)
        {
            _outgoingFactory.TryReturn(ref message);
        }
    }
}
