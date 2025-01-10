using Guppy.Core.Network.Common.Delegates;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Serialization.NetSerializers
{
    internal sealed class RuntimeNetSerializer<T>(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize) : NetSerializer<T>
        where T : notnull
    {
        private readonly NetSerializeDelegate<T> _serialize = serialize;
        private readonly NetDeserializeDelegate<T> _deserialize = deserialize;

        public override T Deserialize(NetDataReader reader) => this._deserialize(reader);

        public override void Serialize(NetDataWriter writer, in T instance) => this._serialize(writer, in instance);
    }
}