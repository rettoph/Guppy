using Guppy.Network.Delegates;
using Guppy.Network.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetSerializers
{
    internal sealed class RuntimeNetSerializerDefinition<T> : NetSerializerDefinition
    {
        public override Type Type => typeof(T);

        public override int InitialSize => 0;

        private NetSerializeDelegate<T> _serialize;
        private NetDeserializeDelegate<T> _deserialize;

        public RuntimeNetSerializerDefinition(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }

        public override NetSerializer BuildNetSerializer(DynamicId id)
        {
            return new NetSerializer<T>(id, this.InitialSize, _serialize, _deserialize);
        }
    }
}
