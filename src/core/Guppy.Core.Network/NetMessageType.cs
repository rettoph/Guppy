﻿using Guppy.Core.Common.Collections;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Peers;
using LiteNetLib;

namespace Guppy.Core.Network
{
    internal abstract class NetMessageType(byte id, Type header, DeliveryMethod defaultDeliveryMethod, byte defaultOutgoingChannel) : INetMessageType
    {
        public byte Id { get; } = id;
        public Type Body { get; } = header;
        public DeliveryMethod DefaultDeliveryMethod { get; } = defaultDeliveryMethod;
        public byte DefaultOutgoingChannel { get; } = defaultOutgoingChannel;

        public abstract INetIncomingMessage CreateIncoming();

        public static NetMessageType Create(
            Type type,
            byte id,
            DeliveryMethod defaultDeliveryMethod,
            byte defaultOutgoingChannel,
            IPeer peer,
            INetSerializerService serializers)
        {
            Type netMessageTypeType = typeof(NetMessageType<>).MakeGenericType(type);

            return (NetMessageType?)Activator.CreateInstance(netMessageTypeType, [id, defaultDeliveryMethod, defaultOutgoingChannel, peer, serializers]) ?? throw new Exception();
        }
    }

    internal sealed class NetMessageType<T> : NetMessageType, INetMessageType<T>
        where T : notnull
    {
        private readonly Peer _peer;
        private readonly INetSerializerService _serializers;
        private readonly Factory<INetIncomingMessage<T>> _incomingFactory;
        private readonly Factory<INetOutgoingMessage<T>> _outgoingFactory;

        public NetMessageType(
            byte id,
            DeliveryMethod defaultDeliveryMethod,
            byte defaultOutgoingChannel,
            IPeer peer,
            INetSerializerService serializers) : base(id, typeof(T), defaultDeliveryMethod, defaultOutgoingChannel)
        {
            if (peer is not Peer casted)
            {
                throw new NotImplementedException();
            }

            _peer = casted;
            _serializers = serializers;
            _incomingFactory = new Factory<INetIncomingMessage<T>>(this.IncomingFactoryMethod);
            _outgoingFactory = new Factory<INetOutgoingMessage<T>>(this.OutgoingFactoryMethod);
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

        public void Recycle(INetIncomingMessage<T> message)
        {
            _incomingFactory.TryReturn(ref message);
        }

        public void Recycle(INetOutgoingMessage<T> message)
        {
            _outgoingFactory.TryReturn(ref message);
        }
    }
}
