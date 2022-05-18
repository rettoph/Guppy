using Guppy.Network.Definitions;
using Guppy.Network.Utilities;
using Guppy.Providers;
using LiteNetLib;
using LiteNetLib.Utils;
using Minnow.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal sealed class NetMessengerProvider : INetMessengerProvider
    {
        private readonly INetSerializerProvider _serializers;
        private readonly ISettingProvider _settings;
        private readonly DynamicIdProvider _ids;
        private readonly DoubleDictionary<Type, ushort, NetMessenger> _messengers;

        public NetMessengerProvider(
            INetSerializerProvider serializers,
            ISettingProvider settings,
            IEnumerable<NetMessengerDefinition> definitions)
        {
            _serializers = serializers;
            _settings = settings;
            _ids = new DynamicIdProvider((ushort)definitions.Count());
            _messengers = _ids.All()
                .Zip(definitions, (id, desc) => desc.BuildNetMessenger(id, _serializers, _settings))
                .ToDoubleDictionary(
                    keySelector1: s => s.Type,
                    keySelector2: s => s.Id.Value);
        }

        public NetIncomingMessage CreateIncoming(NetDataReader reader)
        {
            var id = _ids.Read(reader);
            var messenger = _messengers[id];
            var incoming = messenger.ReadIncoming(reader);
        
            return incoming;
        }
        
        public NetOutgoingMessage<T> CreateOutgoing<T>(NetScope scope, INetTarget target, in T content)
        {
            var messenger = (_messengers[typeof(T)] as NetMessenger<T>)!;
            var outgoing = messenger.CreateOutgoing(scope, target, in content);
        
            return outgoing;
        }

        public NetMessenger<T> GetMessenger<T>()
        {
            return (NetMessenger<T>)_messengers[typeof(T)];
        }

        public NetMessenger GetMessenger(ushort id)
        {
            return _messengers[id];
        }

        public bool TryGetMessenger<T>([MaybeNullWhen(false)] out NetMessenger<T> messenger)
        {
            if (_messengers.TryGetValue(typeof(T), out NetMessenger? value) && value is NetMessenger<T> casted)
            {
                messenger = casted;
                return true;
            }

            messenger = null;
            return false;
        }

        public bool TryGetMessenger(ushort id, [MaybeNullWhen(false)] out NetMessenger messenger)
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
