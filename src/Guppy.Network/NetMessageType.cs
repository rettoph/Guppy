﻿using Guppy.Common.Collections;
using Guppy.Network.Providers;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetMessageType
    {
        public readonly INetId Id;
        public readonly Type Body;
        public readonly DeliveryMethod DeliveryMethod;
        public readonly byte OutgoingChannel;

        protected NetMessageType(INetId id, Type header, DeliveryMethod deliveryMethod, byte outgoingChannel)
        {
            this.Id = id;
            this.Body = header;
            this.DeliveryMethod = deliveryMethod;
            this.OutgoingChannel = outgoingChannel;
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
            INetId id, 
            Type body, 
            DeliveryMethod deliveryMethod, 
            byte outgoingChannel, 
            INetSerializerProvider serializers,
            NetScope scope) : base(id, body, deliveryMethod, outgoingChannel)
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
            return _incomingFactory.GetInstance();
        }

        public INetOutgoingMessage<T> CreateOutgoing()
        {
            return _outgoingFactory.GetInstance();
        }

        internal void Recycle(NetIncomingMessage<T> message)
        {
            _incomingFactory.TryReturnToPool(message);
        }

        internal void Recycle(NetOutgoingMessage<T> message)
        {
            _outgoingFactory.TryReturnToPool(message);
        }
    }
}
