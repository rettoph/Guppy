using Guppy.Attributes;
using Guppy.Extensions.Collection;
using Guppy.Interfaces;
using Guppy.Network.Collections;
using Guppy.Network.Factories;
using Guppy.Network.Peers;
using Guppy.Network.Utilitites.Delegaters;
using Guppy.Network.Utilitites.Options;
using Guppy.Utilities;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        private string _appIdentifier;

        public NetworkServiceLoader(String appIdentifier)
        {
            _appIdentifier = appIdentifier;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MessageTypeDelegater>();
            services.AddSingleton<MessageDelegater>();

            services.AddSingleton<NetPeerConfiguration>(p => new NetPeerConfiguration(_appIdentifier));
            services.AddSingleton<NetworkOptions>();
            services.AddSingleton<GroupFactory>();
            services.AddSingleton<NetPeerFactory>();
            services.AddSingleton<PeerFactory>();
            services.AddSingleton<GroupCollection>();

            AssemblyHelper.GetTypesAssignableFrom<NetPeer>().ForEach(t =>
            { // Add each net peer type as a singleton created via the scene factory...
                services.AddSingleton(t, p => p.GetRequiredService<NetPeerFactory>().Build(t));
            });

            AssemblyHelper.GetTypesAssignableFrom<Peer>().ForEach(t =>
            { // Add each peer type as a singleton created via the scene factory...
                services.AddSingleton(t, p => p.GetRequiredService<PeerFactory>().Build(t));
            });
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            provider.GetRequiredService<NetPeerConfiguration>().EnableMessageType(NetIncomingMessageType.ConnectionApproval);
        }
    }
}
