using Guppy.Network.Loaders.Descriptors;
using Guppy.Network.Utilities;
using LiteNetLib.Utils;
using Minnow.Collections;
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
        private DynamicIdProvider _ids;
        private DoubleDictionary<Type, ushort, NetSerializer> _serializers;

        public NetSerializerProvider(
            IEnumerable<NetSerializerDescriptor> descriptors)
        {
            _ids = new DynamicIdProvider((ushort)descriptors.Count());

            _serializers = _ids.All()
                .Zip(descriptors, (id, desc) => desc.BuildNetSerializer(id))
                .ToDoubleDictionary(
                    keySelector1: s => s.Type,
                    keySelector2: s => s.Id.Value);
        }

        public NetDeserialized Deserialize(NetDataReader reader)
        {
            var id = _ids.Read(reader);
            var serializer = _serializers[id];
            var deserialized = serializer.Deserialize(reader);

            return deserialized;
        }

        public NetSerialized<T> Serialize<T>(in T instance, NetDataWriter writer)
        {
            var type = typeof(T);
            var serializer = (_serializers[type] as NetSerializer<T>)!;

            writer.Put(serializer.Id.Bytes);
            var serialized = serializer.Serialize(in instance, writer);

            return serialized;
        }

        public bool TryGetSerializer<T>([MaybeNullWhen(false)] out NetSerializer<T>? serializer)
        {
            if(_serializers.TryGetValue(typeof(T), out NetSerializer? value) && value is NetSerializer<T> casted)
            {
                serializer = casted;
                return true;
            }

            serializer = null;
            return false;
        }

        public bool TryGetSerializer(ushort id, [MaybeNullWhen(false)] out NetSerializer serializer)
        {
            return _serializers.TryGetValue(id, out serializer);
        }
    }
}
