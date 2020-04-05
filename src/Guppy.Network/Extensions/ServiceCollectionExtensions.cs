using Guppy.Network.Peers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceCollection ConfigureServer(this ServiceCollection services, NetServer server)
        {
            services.AddSingleton(typeof(NetPeer), server);
            services.AddSingleton(typeof(NetServer), server);
            services.AddTypedSingleton<Server, Peer>();

            return services;
        }
    }
}
