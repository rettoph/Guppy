using Guppy.Extensions.DependencyInjection;
using Guppy.Network.Peers;
using Guppy.Utilities.Pools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using Guppy.Network.Utilities;
using Guppy.Network.Utilities.Pools;

namespace Guppy.Network.Extensions
{
    public static class GuppyLoaderExtensions
    {
        #region Client Methods
        private static NetPeerConfiguration ClientConfiguration;

        public static void ConfigureClient(this GuppyLoader guppy, NetPeerConfiguration configuration)
        {
            if (guppy.Initialized)
                throw new Exception("Unable to add services to Guppy! GuppyLoader instance has already been initialized.");
            if (GuppyLoaderExtensions.ClientConfiguration != null)
                throw new Exception("Unable to configure client settings. Client has already been configured.");

            GuppyLoaderExtensions.ClientConfiguration = configuration;

            guppy.Services.TryAddPool<NetClient, NetPeerPool<NetClient>>(p => new NetPeerPool<NetClient>(GuppyLoaderExtensions.ClientConfiguration));
            guppy.Services.AddScoped<NetClient>(p => p.GetConfigurationValueOrCreate<NetClient>("net-peer", peer =>
            {
                p.SetConfigurationValue("net-peer", peer);
            }));
            guppy.Services.TryAddPool<ClientPeer, ReusablePool<ClientPeer>>();
            guppy.Services.AddScoped<ClientPeer>(p => p.GetConfigurationValueOrCreate<ClientPeer>("peer"));
        }
        #endregion

        #region Server Methods
        private static NetPeerConfiguration ServerConfiguration;

        public static void ConfigureServer(this GuppyLoader guppy, NetPeerConfiguration configuration)
        {
            if (guppy.Initialized)
                throw new Exception("Unable to add services to Guppy! GuppyLoader instance has already been initialized.");
            if (GuppyLoaderExtensions.ServerConfiguration != null)
                throw new Exception("Unable to configure server settings. Server has already been configured.");

            GuppyLoaderExtensions.ServerConfiguration = configuration;
            // Ensure that connection approval is always turned on
            GuppyLoaderExtensions.ServerConfiguration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            guppy.Services.TryAddPool<NetServer, NetPeerPool<NetServer>>(p => new NetPeerPool<NetServer>(GuppyLoaderExtensions.ServerConfiguration));
            guppy.Services.AddScoped<NetServer>(p => p.GetConfigurationValueOrCreate<NetServer>("net-peer", peer =>
            {
                p.SetConfigurationValue("net-peer", peer);
            }));
            guppy.Services.TryAddPool<ServerPeer, ReusablePool<ServerPeer>>();
            guppy.Services.AddScoped<ServerPeer>(p => p.GetConfigurationValueOrCreate<ServerPeer>("peer"));
        }
        #endregion
    }
}
