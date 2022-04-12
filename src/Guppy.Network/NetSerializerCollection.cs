using Guppy.Network.Delegates;
using Guppy.Network.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    internal sealed class NetSerializerCollection : List<NetSerializerDescriptor>, INetSerializerCollection
    {
        public INetSerializerCollection Add<T>(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
        {
            this.Add(NetSerializerDescriptor.Create<T>(serialize, deserialize));

            return this;
        }

        public INetSerializerProvider Build()
        {
            return new NetSerializerProvider(this);
        }
    }
}
