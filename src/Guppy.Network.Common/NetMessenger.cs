﻿using Guppy.Network.Loaders;
using Guppy.Network.Providers;
using Guppy.Network.Structs;
using LiteNetLib;
using LiteNetLib.Utils;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetMessenger
    {
        public readonly Type Type;
        public readonly DynamicId Id;
        public readonly DeliveryMethod DeliveryMethod;
        public readonly byte OutgoingChannel;
        public readonly int OutgoingPriority;

        protected internal NetMessenger(
            DynamicId id,
            Type type,
            DeliveryMethod deliveryMethod,
            byte outgoingChannel,
            int outgoingPriority)
        {
            this.Id = id;
            this.Type = type;
            this.DeliveryMethod = deliveryMethod;
            this.OutgoingChannel = outgoingChannel;
            this.OutgoingPriority = outgoingPriority;
        }

        public abstract NetIncomingMessage ReadIncoming(NetDataReader reader);
    }

    public sealed class NetMessenger<T> : NetMessenger
    {
        private INetSerializerProvider _serializers;
        private NetSerializer<T> _serializer;
        private Factory<NetIncomingMessage<T>> _incomingFactory;
        private Factory<NetOutgoingMessage<T>> _outgoingFactory;

        public NetMessenger(
            DynamicId id, 
            DeliveryMethod 
            deliveryMethod, 
            byte outgoingChannel, 
            int outgoingPriority,
            INetSerializerProvider serializers) : base(id, typeof(T), deliveryMethod, outgoingChannel, outgoingPriority)
        {
            _serializers = serializers;

            _incomingFactory = new Factory<NetIncomingMessage<T>>(() => new NetIncomingMessage<T>(_serializers, _serializer, this));
            _outgoingFactory = new Factory<NetOutgoingMessage<T>>(() => new NetOutgoingMessage<T>(_serializers, _serializer, this));

            if(!serializers.TryGetSerializer<T>(out _serializer!))
            {
                throw new Exception($"{nameof(NetMessenger)}<{typeof(T).GetPrettyName()}> - Missing required {nameof(NetSerializer)}. Please ensure a default configuration is defined within an {nameof(INetworkLoader)} implementation.");
            }
        }

        public override NetIncomingMessage<T> ReadIncoming(NetDataReader reader)
        {
            NetIncomingMessage<T> incoming = _incomingFactory.GetInstance();
            incoming.Read(reader);

            return incoming;
        }

        public NetOutgoingMessage<T> CreateOutgoing(in byte roomId, in T content)
        {
            NetOutgoingMessage<T> outgoing = _outgoingFactory.GetInstance();
            outgoing.Write(in roomId, in content);

            return outgoing;
        }

        internal void TryRecycle(NetIncomingMessage<T> incoming)
        {
            _incomingFactory.TryReturnToPool(incoming);
        }

        internal void TryRecycle(NetOutgoingMessage<T> outgoing)
        {
            _outgoingFactory.TryReturnToPool(outgoing);
        }
    }
}
