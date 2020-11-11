using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureServer(this GuppyLoader guppy, NetPeerConfiguration configuration)
        {
            guppy.Services.AddFactory<UserNetConnectionDictionary>(p => new UserNetConnectionDictionary());
            guppy.Services.AddSingleton<UserNetConnectionDictionary>();

            guppy.ConfigurePeer<NetServer, ServerPeer>(
                p => new NetServer(configuration),
                p => new ServerPeer());

            return guppy;
        }

        public static GuppyLoader ConfigureClient(this GuppyLoader guppy, NetPeerConfiguration configuration)
        {
            guppy.ConfigurePeer<NetClient, ClientPeer>(
                p => new NetClient(configuration), 
                p => new ClientPeer());

            return guppy;
        }

        private static void ConfigurePeer<TNetPeer, TPeer>(this GuppyLoader guppy, Func<ServiceProvider, TNetPeer> netPeerFactory, Func<ServiceProvider, TPeer> peerFactory)
            where TPeer : Peer
            where TNetPeer : NetPeer
        {
            // Register related services...
            guppy.Services.AddFactory(typeof(TNetPeer), netPeerFactory);
            guppy.Services.AddFactory(typeof(TPeer), peerFactory);

            guppy.Services.AddSingleton<TNetPeer>();
            guppy.Services.AddSingleton<TPeer>();

            guppy.Services.AddSetup<TNetPeer>((i, p, c) => p.AddLookupRecursive<NetPeer>(c), -20);
            guppy.Services.AddSetup<TPeer>((i, p, c) => p.AddLookupRecursive<Peer>(c), -20);
        }
    }
}
