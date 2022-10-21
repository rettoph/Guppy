using Guppy.Network.Delegates;
using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.NetSerializers
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

        public override T Deserialize(NetDataReader reader, INetSerializerProvider serializers)
        {
            return _deserialize(reader, serializers);
        }

        public override void Serialize(NetDataWriter writer, INetSerializerProvider serializers, in T instance)
        {
            _serialize(writer, serializers, in instance);
        }
    }
}
