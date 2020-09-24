using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.Network.Utilities;
using Guppy.Network.Utilities.Messages;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            // Configure service factories...
            services.AddFactory<User>(p => new User());
            services.AddFactory<ServiceList<User>>(p => new ServiceList<User>());
            services.AddFactory<ServiceList<Group>>(p => new ServiceList<Group>());
            services.AddFactory<PeerData>(p => new PeerData());
            services.AddFactory<NetPeer>(p => p.GetService<PeerData>().NetPeer);
            services.AddFactory<Peer>(p => p.GetService<PeerData>().Peer);
            services.AddFactory<Group>(p => p.GetService<Peer>().GroupFactory());

            // Setup service scopes...
            services.AddTransient<User>();
            services.AddTransient<ServiceList<User>>();
            services.AddSingleton<ServiceList<Group>>();
            services.AddSingleton<PeerData>();
            services.AddSingleton<NetPeer>();
            services.AddSingleton<Peer>();
            services.AddSingleton<Group>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
