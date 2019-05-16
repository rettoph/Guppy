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
        public static void ConfigureNetwork<TPeer, TGroup>(this GuppyLoader guppy, Func<IServiceProvider, TPeer> peerFactory, NetworkSceneDriver networkSceneDriver)
            where TPeer : Peer
            where TGroup : Group
        {
            guppy.Services.AddScene<NetworkScene>();
            guppy.Services.AddSingleton<TPeer>(peerFactory);
            guppy.Services.AddSingleton<Peer>(GuppyExtensions.GetPeer<TPeer>);
            guppy.Services.AddScoped<TGroup>(GuppyExtensions.GetGroup<TGroup>);
            guppy.Services.AddScoped<Group>(GuppyExtensions.GetGroup<Group>);
            guppy.Services.AddSingleton<NetworkSceneDriver>(networkSceneDriver);
        }

        private static Peer GetPeer<TPeer>(IServiceProvider provider)
            where TPeer : Peer
        {
            return provider.GetRequiredService<TPeer>();
        }

        private static TGroup GetGroup<TGroup>(IServiceProvider provider)
            where TGroup : Group
        {
            return provider.GetRequiredService<NetworkScene>().group as TGroup;
        }
    }
}
