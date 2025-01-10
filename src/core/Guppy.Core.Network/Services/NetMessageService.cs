using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Services
{
    internal sealed class NetMessageService : INetMessageService
    {
        private readonly IPeer _peer;

        private readonly Dictionary<byte, NetMessageType> _messageIds;
        private readonly Dictionary<Type, NetMessageType> _messageTypes;

        public NetMessageService(IPeer peer, INetSerializerService serializers, IEnumerable<NetMessageTypeDefinition> definitions)
        {
            this._peer = peer;

            this._messageIds = default!;
            this._messageTypes = default!;

            byte id = 0;
            IList<NetMessageType> messages = definitions.Select(definition =>
            {
                return NetMessageType.Create(definition.Body, id++, definition.DefaultDeliveryMethod, definition.DefaultOutgoingChannel, peer, serializers);
            }).ToList();

            this._messageIds = messages.ToDictionary(x => x.Id, x => x);
            this._messageTypes = messages.ToDictionary(x => x.Body, x => x);
        }

        public INetMessageType GetById(byte id)
        {
            return this._messageIds[id];
        }

        public INetMessageType<T> Get<T>()
            where T : notnull
        {
            try
            {
                return (NetMessageType<T>)this._messageTypes[typeof(T)];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException($"{nameof(NetMessageService)}::{nameof(Get)} - No {nameof(NetMessageType)} registered for type {typeof(T).Name}", e);
            }
        }

        public INetIncomingMessage Read(NetPeer sender, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            byte id = reader.GetByte();
            var message = this._messageIds[id].CreateIncoming();
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
    }
}