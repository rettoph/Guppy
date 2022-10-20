using Guppy.Network.Providers;
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
        public abstract NetSerializer Build(INetId id);
    }

    public abstract class NetSerializerDefinition<T> : NetSerializerDefinition
    {
        public override Type Type { get; } = typeof(T);

        public abstract void Serialize(NetDataWriter writer, INetDatumProvider datum, in T instance);

        public abstract void Deserialize(NetDataReader reader, INetDatumProvider datum, out T instance);

        public override NetSerializer Build(INetId id)
        {
            return new NetSerializer<T>(id, this.Serialize, this.Deserialize);
        }
    }
}
