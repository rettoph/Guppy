using Guppy.Network.Peers;
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
            guppy.Services.AddSingleton<NetServer>(p => new NetServer(configuration));
            guppy.Services.AddSingleton<Server>(p => new Server());

            return guppy;
        }

        public static GuppyLoader ConfigureClient(this GuppyLoader guppy, NetPeerConfiguration configuration)
        {
            guppy.Services.AddSingleton<NetClient>(p => new NetClient(configuration));
            guppy.Services.AddSingleton<Client>(p => new Client());

            return guppy;
        }
    }
}
