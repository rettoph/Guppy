using Guppy.Network.Definitions;
using Guppy.Network.Loaders;
using Guppy.Network.Providers;
using Guppy.Network.Structs;
using LiteNetLib;
using LiteNetLib.Utils;
using Minnow.Collections;
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
        private readonly INetSerializerProvider _serializers;
        private readonly NetSerializer<T> _serializer;
        private readonly Factory<NetIncomingMessage<T>> _incomingFactory;
        private readonly Factory<NetOutgoingMessage<T>> _outgoingFactory;

        public NetMessenger(
            DynamicId id, 
            DeliveryMethod 
            deliveryMethod, 
            byte outgoingChannel, 
            int outgoingPriority,
            INetSerializerProvider serializers) : base(id, typeof(T), deliveryMethod, outgoingChannel, outgoingPriority)
        {
            _serializers = serializers;

            if (!serializers.TryGetSerializer<T>(out _serializer!))
            {
                throw new Exception($"{nameof(NetMessenger)}<{typeof(T).GetPrettyName()}> - Missing required {nameof(NetSerializer)}. Please ensure a matching {nameof(NetSerializerDefinition)} is defined.");
            }

            _incomingFactory = new Factory<NetIncomingMessage<T>>(() => new NetIncomingMessage<T>(_serializers, _serializer, this));
            _outgoingFactory = new Factory<NetOutgoingMessage<T>>(() => new NetOutgoingMessage<T>(_serializers, _serializer, this));
        }

        public override NetIncomingMessage<T> ReadIncoming(NetDataReader reader)
        {
            NetIncomingMessage<T> incoming = _incomingFactory.GetInstance();
            incoming.Read(reader);

            return incoming;
        }

        public NetOutgoingMessage<T> CreateOutgoing(Room room, in T content)
        {
            NetOutgoingMessage<T> outgoing = _outgoingFactory.GetInstance();
            outgoing.Write(room, in content);

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
