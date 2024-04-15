using Guppy.Core.Network.Definitions;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Services
{
    internal sealed class NetMessageService : INetMessageService
    {
        private readonly IPeer _peer;
        private readonly INetSerializerService _serializers;
        private readonly IEnumerable<NetMessageTypeDefinition> _definitions;

        private INetGroup _scope;
        private IDictionary<byte, NetMessageType> _messageIds;
        private IDictionary<Type, NetMessageType> _messageTypes;

        public NetMessageService(
            IPeer peer,
            INetSerializerService serializers,
            IEnumerable<NetMessageTypeDefinition> definitions)
        {
            _peer = peer;
            _serializers = serializers;
            _definitions = definitions;

            _messageIds = default!;
            _messageTypes = default!;
            _scope = default!;

            byte id = 0;
            IList<NetMessageType> messages = _definitions.Select(definition =>
            {
                return definition.Build(
                    id: id++,
                    peer: _peer,
                    serializers: _serializers);
            }).ToList();

            _messageIds = messages.ToDictionary(x => x.Id, x => x);
            _messageTypes = messages.ToDictionary(x => x.Body, x => x);
        }

        public NetMessageType GetById(byte id)
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

        public INetIncomingMessage Read(NetPeer sender, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            byte id = reader.GetByte();
            var message = _messageIds[id].CreateIncoming();
            message.Read(sender, reader, ref channel, ref deliveryMethod);

            return message;
        }

        public INetOutgoingMessage<T> Create<T>(in INetGroup group, in T body)
            where T : notnull
        {
            var message = this.Get<T>().CreateOutgoing();
            message.Write(in group, in body);

            return message;
        }

        public INetOutgoingMessage<T> Create<T>(in byte groupId, in T body) where T : notnull
        {
            return this.Create(_peer.Groups.GetById(groupId), in body);
        }
    }
}
