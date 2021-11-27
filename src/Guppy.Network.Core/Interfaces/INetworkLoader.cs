using Guppy.Network.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface INetworkLoader
    {
        void ConfigureNetwork(PacketServiceBuilder packets);
    }
}
