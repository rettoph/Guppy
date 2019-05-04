using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions.Guppy
{
    public static class GuppyExtensions
    {
        public static void ConfigureNetwork(this GuppyLoader guppy, Func<IServiceProvider, Peer> peerFactory, NetworkSceneDriver networkSceneDriver)
        {
            guppy.Services.AddSingleton(peerFactory);
            guppy.Services.AddSingleton(GuppyExtensions.GetPeerAs<ClientPeer>);
            guppy.Services.AddSingleton(GuppyExtensions.GetPeerAs<ServerPeer>);
            guppy.Services.AddSingleton<NetworkSceneDriver>(networkSceneDriver);
        }

        private static TPeer GetPeerAs<TPeer>(IServiceProvider provider)
            where TPeer : Peer
        {
            var peer = provider.GetRequiredService<Peer>();

            if (peer is TPeer)
                return peer as TPeer;
            else
                throw new Exception($"Peer instance is {peer.GetType().Name}, but {typeof(TPeer).Name} requested.");
        }
    }
}
