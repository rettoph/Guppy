using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages
{
    public sealed class DisposeNetworkEntityMessage : NetworkEntityMessage<DisposeNetworkEntityMessage>
    {
        internal static bool Filter(ServiceProvider p, NetworkMessageConfiguration network)
        {
            if (p.GetService<Peer>() is not ClientPeer)
            {
                return false;
            }

            if (p.GetService<Scene>() is null)
            {
                return false;
            }

            return true;
        }
    }
}
