using Guppy.Network.Loaders.Descriptors;
using Guppy.Network.Utilities;
using LiteNetLib.Utils;
using Minnow.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal sealed class NetMessengerProvider : INetMessengerProvider
    {
        private INetSerializerProvider _serializers;
        private DynamicIdProvider _ids;
        private DoubleDictionary<Type, ushort, NetMessenger> _messengers;

        public NetMessengerProvider(
            INetSerializerProvider serializers,
            IEnumerable<NetMessengerDescriptor> descriptors)
        {
            _serializers = serializers;
            _ids = new DynamicIdProvider((ushort)descriptors.Count());
            _messengers = _ids.All()
                .Zip(descriptors, (id, desc) => desc.BuildNetMessenger(id, _serializers))
                .ToDoubleDictionary(
                    keySelector1: s => s.Type,
                    keySelector2: s => s.Id.Value);
        }

        public NetIncomingMessage ReadIncoming(NetDataReader reader)
        {
            var id = _ids.Read(reader);
            var messenger = _messengers[id];
            var incoming = messenger.ReadIncoming(reader);

            return incoming;
        }

        public NetOutgoingMessage<T> CreateOutgoing<T>(Room room, in T content)
        {
            var messenger = (_messengers[typeof(T)] as NetMessenger<T>)!;
            var outgoing = messenger.CreateOutgoing(room, in content);

            return outgoing;
        }

        public bool TryGetMessenger<T>(out NetMessenger<T>? messenger)
        {
            if (_messengers.TryGetValue(typeof(T), out NetMessenger value) && value is NetMessenger<T> casted)
            {
                messenger = casted;
                return true;
            }

            messenger = null;
            return false;
        }

        public bool TryGetMessenger(ushort id, out NetMessenger messenger)
        {
            return _messengers.TryGetValue(id, out messenger);
        }

        public IEnumerator<NetMessenger> GetEnumerator()
        {
            return _messengers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
