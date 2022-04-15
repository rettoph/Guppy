using Guppy.Network.Delegates;
using Guppy.Network.Loaders.Descriptors;
using Guppy.Network.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders.Collections
{
    public interface INetSerializerCollection : IList<NetSerializerDescriptor>
    {
        INetSerializerCollection Add<T>(NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize);
    }
}
