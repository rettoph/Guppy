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

        private NetScope _netScope;
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
            _netScope = default!;
        }

        void INetMessageService.Initialize(NetScope netScope)
        {
            byte id = 0;
            IList<NetMessageType> messages = _definitions.Select(definition =>
            {
                return definition.Build(
                    id: id++,
                    serializers: _serializers,
                    netScope: netScope);
            }).ToList();

            _netScope = netScope;
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
            var message = this.Get<T>().CreateOutgoing();
            message.Write(in body);

            if (_netScope.Peer?.Users.Current?.NetPeer is null)
            {
                return message;
            }

            message.AddRecipient(_netScope.Peer!.Users.Current!.NetPeer!);
            return message;
        }
    }
}
