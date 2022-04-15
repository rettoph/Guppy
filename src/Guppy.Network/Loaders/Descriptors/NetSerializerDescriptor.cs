using Guppy.Network.Delegates;
using Guppy.Network.Structs;

namespace Guppy.Network.Loaders.Descriptors
{
    public abstract class NetSerializerDescriptor
    {
        public abstract NetSerializer BuildNetSerializer(DynamicId id);

        public static NetSerializerDescriptor Create<T>(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
        {
            return new NetSerializerDescriptor<T>(serialize, deserialize);
        }
    }

    internal sealed class NetSerializerDescriptor<T> : NetSerializerDescriptor
    {
        private NetSerializeDelegate<T> _serialize;
        private NetDeserializeDelegate<T> _deserialize;

        public NetSerializerDescriptor(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }

        public override NetSerializer BuildNetSerializer(DynamicId id)
        {
            return new NetSerializer<T>(id, _serialize, _deserialize);
        }
    }
}