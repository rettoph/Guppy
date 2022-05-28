using Guppy.Network.Components;
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
    internal sealed class NetMessageProvider : INetMessageProvider
    {
        private readonly INetSerializerProvider _serializers;
        private readonly ISettingProvider _settings;
        private readonly DynamicIdProvider _ids;
        private readonly DoubleDictionary<Type, ushort, NetMessageFactory> _factories;

        public NetMessageProvider(
            INetSerializerProvider serializers,
            ISettingProvider settings,
            IEnumerable<NetMessageFactoryDefinition> definitions)
        {
            _serializers = serializers;
            _settings = settings;
            _ids = new DynamicIdProvider((ushort)definitions.Count());
            _factories = _ids.All()
                .Zip(definitions, (id, desc) => desc.BuildNetMessenger(id, _serializers, _settings))
                .ToDoubleDictionary(
                    keySelector1: s => s.Type,
                    keySelector2: s => s.Id.Value);
        }

        public NetIncomingMessage CreateIncoming(NetPeer sender, NetDataReader reader)
        {
            var id = _ids.Read(reader);
            var messenger = _factories[id];
            var incoming = messenger.CreateIncoming(sender, reader);
        
            return incoming;
        }
        
        public NetOutgoingMessage<T> CreateOutgoing<T>(NetMessenger messenger, in T content)
        {
            var factory = (_factories[typeof(T)] as NetMessageFactory<T>)!;
            var outgoing = factory.CreateOutgoing(messenger, in content);
        
            return outgoing;
        }

        public NetMessageFactory<T> GetFactory<T>()
        {
            return (NetMessageFactory<T>)_factories[typeof(T)];
        }

        public NetMessageFactory GetFactory(ushort id)
        {
            return _factories[id];
        }

        public bool TryGetFactory<T>([MaybeNullWhen(false)] out NetMessageFactory<T> messenger)
        {
            if (_factories.TryGetValue(typeof(T), out NetMessageFactory? value) && value is NetMessageFactory<T> casted)
            {
                messenger = casted;
                return true;
            }

            messenger = null;
            return false;
        }

        public bool TryGetFactory(ushort id, [MaybeNullWhen(false)] out NetMessageFactory messenger)
        {
            return _factories.TryGetValue(id, out messenger);
        }

        public IEnumerator<NetMessageFactory> GetEnumerator()
        {
            return _factories.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
