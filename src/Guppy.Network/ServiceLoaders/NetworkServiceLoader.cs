using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Network.Collections;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<User>(p => new User());
            services.AddTransient<UserCollection>(p => new UserCollection());
            services.AddTransient<UserGroupCollection>(p => new UserGroupCollection());
            services.AddSingleton<GroupCollection>(p => new GroupCollection());
            services.AddSingleton<PeerData>(p => new PeerData());
            services.AddSingleton<NetPeer>(p => p.GetService<PeerData>().NetPeer);
            services.AddSingleton<Peer>(p => p.GetService<PeerData>().Peer);
            services.AddSingleton<Group>(p => p.GetService<Peer>().GroupFactory());
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
