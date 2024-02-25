using Guppy.Network.Definitions;
using Guppy.Network.Providers;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Network.Services
{
    internal sealed class NetMessageService : INetMessageService
    {
        private readonly INetSerializerProvider _serializers;
        private readonly IEnumerable<NetMessageTypeDefinition> _definitions;

        private INetScope _scope;
        private IDictionary<byte, NetMessageType> _messageIds;
        private IDictionary<Type, NetMessageType> _messageTypes;

        public NetMessageService(
            INetSerializerProvider serializers,
            IEnumerable<NetMessageTypeDefinition> definitions)
        {
            _serializers = serializers;
            _definitions = definitions;

            _messageIds = default!;
            _messageTypes = default!;
            _scope = default!;
        }

        void INetMessageService.Initialize(INetScope netScope)
        {
            byte id = 0;
            IList<NetMessageType> messages = _definitions.Select(definition =>
            {
                return definition.Build(
                    id: id++,
                    serializers: _serializers,
                    netScope: netScope);
            }).ToList();

            _scope = netScope;
            _messageIds = messages.ToDictionary(x => x.Id, x => x);
            _messageTypes = messages.ToDictionary(x => x.Body, x => x);
        }

        public NetMessageType Get(byte id)
        {
            return _messageIds[id];
        }

        public NetMessageType<T> Get<T>()
            where T : notnull
        {
            try
            {
                return (NetMessageType<T>)_messageTypes[typeof(T)];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException($"{nameof(NetMessageService)}::{nameof(Get)} - No {nameof(NetMessageType)} registered for type {typeof(T).Name}", e);
            }
        }

        public INetIncomingMessage Read(NetPeer? peer, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            byte id = reader.GetByte();
            var message = _messageIds[id].CreateIncoming();
            message.Read(peer, reader, ref channel, ref deliveryMethod);

            return message;
        }

        public INetOutgoingMessage<T> Create<T>(in T body)
            where T : notnull
        {
            // In an attempt to make client users have no peer attached ive broken client message sending, as this method made sure
            // to add the server peer as a default recipient for client messages
            // I would really like to make client/server specific implementations for this service, NetScope, and others

            var message = this.Get<T>().CreateOutgoing();
            message.Write(in body);

            return message;
        }
    }
}
