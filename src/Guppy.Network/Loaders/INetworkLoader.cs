using Guppy.Loaders;
using Guppy.Network.Loaders.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders
{
    public interface INetworkLoader : IGuppyLoader
    {
        void ConfigureNetSerializers(INetSerializerCollection serializers);
        void ConfigureNetMessengers(INetMessengerCollection messengers);
    }
}
