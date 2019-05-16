using Guppy.Extensions;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions.Guppy
{
    public static class GuppyExtensions
    {
        public static void ConfigureNetwork<TPeer>(this GuppyLoader guppy, Func<IServiceProvider, TPeer> peerFactory, NetworkSceneDriver networkSceneDriver)
            where TPeer : Peer
        {
            guppy.Services.AddScene<NetworkScene>();
            guppy.Services.AddSingleton<TPeer>(peerFactory);
            guppy.Services.AddSingleton<Peer>(GuppyExtensions.GetPeer<TPeer>);
            guppy.Services.AddSingleton<NetworkSceneDriver>(networkSceneDriver);
        }

        private static Peer GetPeer<TPeer>(IServiceProvider provider)
            where TPeer : Peer
        {
            return provider.GetRequiredService<TPeer>();
        }
    }
}
