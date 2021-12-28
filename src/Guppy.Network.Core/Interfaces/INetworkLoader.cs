using Guppy.Interfaces;
using Guppy.Network.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    public interface INetworkLoader : IGuppyLoader
    {
        void ConfigureNetwork(NetworkProviderBuilder network);
    }
}
