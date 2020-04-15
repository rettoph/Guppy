using Guppy.Network.Peers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureServer(this GuppyLoader guppy, String appIdentifier)
        {
            guppy.Services.AddSingleton<NetPeerConfiguration>(p => new NetPeerConfiguration(appIdentifier));
            guppy.Services.AddSingleton<NetPeer>(p => throw new Exception("No NetPeer instance created."));
            guppy.Services.AddSingleton<NetServer>(p => new NetServer(p.GetService<NetPeerConfiguration>()), 0, typeof(NetPeer));
            guppy.Services.AddSingleton<Server>(p => new Server());

            return guppy;
        }

        public static GuppyLoader ConfigureClient(this GuppyLoader guppy, String appIdentifier)
        {
            guppy.Services.AddSingleton<NetPeerConfiguration>(p => new NetPeerConfiguration(appIdentifier));
            guppy.Services.AddSingleton<NetPeer>(p => throw new Exception("No NetPeer instance created."));
            guppy.Services.AddSingleton<NetClient>(p => new NetClient(p.GetService<NetPeerConfiguration>()), 0, typeof(NetPeer));
            guppy.Services.AddSingleton<Client>(p => new Client());

            return guppy;
        }
    }
}
