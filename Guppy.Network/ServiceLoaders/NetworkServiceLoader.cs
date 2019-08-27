using Guppy.Attributes;
using Guppy.Extensions.Collection;
using Guppy.Interfaces;
using Guppy.Network.Factories;
using Guppy.Utilities;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            services.AddSingleton<NetPeerConfiguration>(new NetPeerConfiguration(_appIdentifier));
            services.AddSingleton<GroupFactory>();
            services.AddSingleton<NetPeerFactory>();
            services.AddSingleton<PeerFactory>();

            AssemblyHelper.GetTypesAssignableFrom<NetPeer>().ForEach(t =>
            { // Add each scene type as a singleton created via the scene factory...
                services.AddSingleton(t, p => p.GetRequiredService<NetPeerFactory>().Build(t));
            });

            services.Configure<NetPeerConfiguration>(config =>
            { // Setup the default NetPeerConfiguration vaues...
                config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            });
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            //  throw new NotImplementedException();
        }
    }
}
