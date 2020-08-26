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
            Func<ServiceProvider, TNetPeer> netPeer = p =>
            {
                var data = p.GetService<PeerData>();

                if (data.NetPeer == null)
                    data.NetPeer = netPeerFactory(p);

                return data.NetPeer as TNetPeer;
            };

            Func<ServiceProvider, TPeer> peer = p =>
            {
                var data = p.GetService<PeerData>();

                if (data.Peer == null)
                    data.Peer = peerFactory(p);

                return data.Peer as TPeer;
            };

            // Register related services...
            guppy.Services.AddFactory(typeof(TNetPeer), netPeer);
            guppy.Services.AddFactory(typeof(TPeer), peer);

            guppy.Services.AddSingleton<TNetPeer>(cacheType: typeof(NetPeer));
            guppy.Services.AddSingleton<TPeer>(cacheType: typeof(Peer));
        }
    }
}
