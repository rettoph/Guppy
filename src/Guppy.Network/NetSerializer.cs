using Guppy.Common.Collections;
using Guppy.Network.Delegates;
using LiteNetLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetSerializer
    {
        public readonly Type Type;
        public readonly INetId Id;

        internal NetSerializer(Type type, INetId id)
        {
            this.Type = type;
            this.Id = id;
        }

        internal abstract NetDatumType BuildDatumType();
    }

    public sealed class NetSerializer<T> : NetSerializer
    {
        public NetSerializeDelegate<T> Serialize;
        public NetDeserializeDelegate<T> Deserialize;

        internal NetSerializer(INetId id, NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize) : base(typeof(T), id)
        {
            this.Serialize = serialize;
            this.Deserialize = deserialize;
        }

        internal override NetDatumType BuildDatumType()
        {
            return new NetDatumType<T>(this);
        }
    }
}
