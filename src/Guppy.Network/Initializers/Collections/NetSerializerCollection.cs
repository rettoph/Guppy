using Guppy.Network.Delegates;
using Guppy.Network.Loaders.Collections;
using Guppy.Network.Loaders.Descriptors;
using Guppy.Network.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Initializers.Collections
{
    internal sealed class NetSerializerCollection : List<NetSerializerDescriptor>, INetSerializerCollection
    {
        public INetSerializerCollection Add<T>(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
        {
            this.Add(NetSerializerDescriptor.Create<T>(serialize, deserialize));

            return this;
        }

        public INetSerializerProvider BuildNetSerializerProvider()
        {
            return new NetSerializerProvider(this);
        }
    }
}
