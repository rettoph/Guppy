using Guppy.Common.Collections;
using Guppy.Network.Definitions;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal sealed class NetMessageProvider : INetMessageProvider
    {
        private DoubleDictionary<INetId, Type, NetMessageType> _types;

        public NetMessageProvider(
            INetSerializerProvider serializers,
            INetDatumProvider data,
            IEnumerable<NetMessageTypeDefinition> definitions)
        {
            _types = new DoubleDictionary<INetId, Type, NetMessageType>();

            byte id = 0;
            foreach(NetMessageTypeDefinition definition in definitions)
            {
                var type = definition.BuildType(NetId.Create(id), serializers, data);
                id += 1;

                _types.TryAdd(type.Id, type.Header, type);
            }
        }

        public INetIncomingMessage Read(NetDataReader reader)
        {
            var id = NetId.Byte.Read(reader);
            var message = _types[id].CreateIncoming();
            message.Read(reader);

            return message;
        }

        public NetOutgoingMessage<TBody> Create<TBody>(in TBody body, NetScope scope)
        {
            if(_types[typeof(TBody)] is NetMessageType<TBody> factory)
            {
                var message = factory.CreateOutgoing();
                message.Write(in body, scope);

                scope.Publish(in message);

                return message;
            }

            throw new NotImplementedException();
        }
    }
}
