using Guppy.Core.Network.Delegates;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Serialization.NetSerializers
{
    internal sealed class RuntimeNetSerializer<T> : NetSerializer<T>
        where T : notnull
    {
        private readonly NetSerializeDelegate<T> _serialize;
        private readonly NetDeserializeDelegate<T> _deserialize;

        public RuntimeNetSerializer(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }

        public override T Deserialize(NetDataReader reader)
        {
            return _deserialize(reader);
        }

        public override void Serialize(NetDataWriter writer, in T instance)
        {
            _serialize(writer, in instance);
        }
    }
}
