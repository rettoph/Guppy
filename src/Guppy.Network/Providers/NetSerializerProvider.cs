using Guppy.Network.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal sealed class NetSerializerProvider : INetSerializerProvider
    {
        private Dictionary<Type, NetSerializer> _serializers;

        public NetSerializerProvider(IEnumerable<NetSerializerDefinition> definitions)
        {
            _serializers = new Dictionary<Type, NetSerializer>(definitions.Count());

            byte id = 0;
            foreach(var definition in definitions)
            {
                _serializers.Add(definition.Type, definition.Build(NetId.Create(id)));
                id += 1;
            }
        }

        public NetSerializer<T> Get<T>()
        {
            return (NetSerializer<T>)_serializers[typeof(T)];
        }

        public bool TryGet<T>([MaybeNullWhen(false)] out NetSerializer<T> serializer)
        {
            if(_serializers.TryGetValue(typeof(T), out NetSerializer? uncasted) && uncasted is NetSerializer<T> casted)
            {
                serializer = casted;
                return true;
            }

            serializer = null;
            return false;
        }

        public IEnumerable<NetDatumType> BuildDatumTypes()
        {
            foreach(NetSerializer serializer in _serializers.Values)
            {
                yield return serializer.BuildDatumType();
            }
        }
    }
}
