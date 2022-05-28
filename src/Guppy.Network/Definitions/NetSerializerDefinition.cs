using Guppy.Network.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions
{
    public abstract class NetSerializerDefinition
    {
        public abstract Type Type { get; }
        public abstract int InitialSize { get; }
        public abstract NetSerializer BuildNetSerializer(DynamicId id);
    }

    public abstract class NetSerializerDefinition<T> : NetSerializerDefinition
    {
        public override Type Type { get; } = typeof(T);

        public override int InitialSize { get; } = 0;

        public abstract void Serialize(NetDataWriter writer, in T instance);

        public abstract void Deserialize(NetDataReader reader, out T instance);

        public override NetSerializer BuildNetSerializer(DynamicId id)
        {
            return new NetSerializer<T>(id, this.InitialSize, this.Serialize, this.Deserialize);
        }
    }
}
