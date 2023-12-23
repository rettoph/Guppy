using Guppy.Common.Collections;
using Guppy.Network.Providers;
using LiteNetLib;
using System.Runtime.CompilerServices;

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
        private readonly NetScope _scope;
        private readonly INetSerializerProvider _serializers;
        private readonly Factory<INetIncomingMessage<T>> _incomingFactory;
        private readonly Factory<INetOutgoingMessage<T>> _outgoingFactory;

        public NetMessageType(
            byte id,
            Type body,
            DeliveryMethod defaultDeliveryMethod,
            byte defaultOutgoingChannel,
            INetSerializerProvider serializers,
            NetScope scope) : base(id, body, defaultDeliveryMethod, defaultOutgoingChannel)
        {
            _scope = scope;
            _serializers = serializers;
            _incomingFactory = new Factory<INetIncomingMessage<T>>(this.IncomingFactoryMethod);
            _outgoingFactory = new Factory<INetOutgoingMessage<T>>(this.OutgoingFactoryMethod);
        }

        private INetIncomingMessage<T> IncomingFactoryMethod()
        {
            return new NetIncomingMessage<T>(this, _scope, _serializers);
        }

        private INetOutgoingMessage<T> OutgoingFactoryMethod()
        {
            return new NetOutgoingMessage<T>(this, _scope, _serializers);
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
            _incomingFactory.TryReturn(ref Unsafe.AsRef<INetIncomingMessage<T>>(message));
        }

        internal void Recycle(NetOutgoingMessage<T> message)
        {
            _outgoingFactory.TryReturn(ref Unsafe.AsRef<INetOutgoingMessage<T>>(message));
        }
    }
}
