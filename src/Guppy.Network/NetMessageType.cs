using Guppy.Common.Collections;
using Guppy.Network.Providers;
using Guppy.Network.Structs;
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
        public readonly NetId Id;
        public readonly Type Header;
        public readonly DeliveryMethod DeliveryMethod;
        public readonly byte OutgoingChannel;

        protected NetMessageType(NetId id, Type header, DeliveryMethod deliveryMethod, byte outgoingChannel)
        {
            this.Id = id;
            this.Header = header;
            this.DeliveryMethod = deliveryMethod;
            this.OutgoingChannel = outgoingChannel;
        }

        public abstract INetIncomingMessage CreateIncoming();
    }

    public sealed class NetMessageType<THeader> : NetMessageType
    {
        private readonly NetSerializer<THeader> _serializer;
        private readonly INetDatumProvider _data;
        private readonly Factory<NetIncomingMessage<THeader>> _incomingFactory;
        private readonly Factory<NetOutgoingMessage<THeader>> _outgoingFactory;

        public NetMessageType(
            NetId id, 
            Type header, 
            DeliveryMethod deliveryMethod, 
            byte outgoingChannel, 
            INetSerializerProvider serializers, 
            INetDatumProvider data) : base(id, header, deliveryMethod, outgoingChannel)
        {
            _serializer = serializers.Get<THeader>();
            _data = data;
            _incomingFactory = new Factory<NetIncomingMessage<THeader>>(this.IncomingFactoryMethod);
            _outgoingFactory = new Factory<NetOutgoingMessage<THeader>>(this.OutgoingFactoryMethod);
        }

        private NetIncomingMessage<THeader> IncomingFactoryMethod()
        {
            return new NetIncomingMessage<THeader>(this, _serializer, _data);
        }

        private NetOutgoingMessage<THeader> OutgoingFactoryMethod()
        {
            return new NetOutgoingMessage<THeader>(this, _serializer, _data);
        }

        public override NetIncomingMessage<THeader> CreateIncoming()
        {
            return _incomingFactory.GetInstance();
        }

        public NetOutgoingMessage<THeader> CreateOutgoing()
        {
            return _outgoingFactory.GetInstance();
        }

        internal void Recycle(NetIncomingMessage<THeader> message)
        {
            _incomingFactory.TryReturnToPool(message);
        }

        internal void Recycle(NetOutgoingMessage<THeader> message)
        {
            _outgoingFactory.TryReturnToPool(message);
        }
    }
}
