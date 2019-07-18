using Guppy.Extensions.DependencyInjection;
using Guppy.Network.Configurations;
using Guppy.Network.Drivers;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions.Guppy
{
    public static class GuppyExtensions
    {
        private static Peer GetPeer(IServiceProvider provider)
        {
            return provider.GetService(provider.GetRequiredService<NetworkConfiguration>().Peer) as Peer;
        }

        public static void ConfigureServer(this GuppyLoader guppy)
        {
            guppy.Services.AddSingleton<ServerPeer>();
            guppy.Services.AddSingleton<Peer>(GuppyExtensions.GetPeer);
            guppy.Services.AddSingleton<NetServer>(p => p.GetRequiredService<NetPeer>() as NetServer);
            guppy.Services.AddDriver<NetworkScene, ServerNetworkSceneDriver>();

            guppy.Services.AddSingleton<ServerGroup>();

            guppy.Services.AddSingleton<NetworkConfiguration>(NetworkConfiguration.Server);
        }

        public static void ConfigureClient(this GuppyLoader guppy)
        {
            guppy.Services.AddSingleton<ClientPeer>();
            guppy.Services.AddSingleton<Peer>(GuppyExtensions.GetPeer);
            guppy.Services.AddSingleton<NetClient>(p => p.GetRequiredService<NetPeer>() as NetClient);
            guppy.Services.AddDriver<NetworkScene, ClientNetworkSceneDriver>();

            guppy.Services.AddSingleton<ClientGroup>();

            guppy.Services.AddSingleton<NetworkConfiguration>(NetworkConfiguration.Client);
        }
    }
}
