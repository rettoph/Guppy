using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Network.Channels;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Peers;
using Guppy.Network.Pipes;
using Guppy.Network.Security;
using Guppy.Network.Services;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    public class NetworkServiceLoaders : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<NetOutgoingMessageService>(p => new NetOutgoingMessageService());
            services.AddFactory<ChannelList>(p => new ChannelList());
            services.AddFactory<PipeList>(p => new PipeList());
            services.AddFactory<UserList>(p => new UserList());
            services.AddFactory<ServiceList<INetworkService>>(p => new ServiceList<INetworkService>());
            services.AddFactory<ServerPeer>(p => new ServerPeer());
            services.AddFactory<NetServer>(p => new NetServer(p.GetService<NetPeerConfiguration>()));
            services.AddFactory<ClientPeer>(p => new ClientPeer());
            services.AddFactory<NetClient>(p => new NetClient(p.GetService<NetPeerConfiguration>()));
            services.AddFactory<NetPeerConfiguration>(p => new NetPeerConfiguration("guppy"));
            services.AddFactory<ServerChannel>(p => new ServerChannel());
            services.AddFactory<ClientChannel>(p => new ClientChannel());
            services.AddFactory<IPipe>(p => new Pipe());
            services.AddFactory<IUser>(p => new User());

            services.AddSingleton<NetOutgoingMessageService>();
            services.AddSingleton<ChannelList>();
            services.AddScoped<PipeList>();
            services.AddSingleton<UserList>();
            services.AddTransient<UserList>(GuppyNetworkCoreConstants.ServiceConfigurations.TransientUserList);
            services.AddScoped<ServiceList<INetworkService>>();
            services.AddSingleton<ServerPeer>();
            services.AddSingleton<NetServer>();
            services.AddSingleton<ClientPeer>();
            services.AddSingleton<NetClient>();
            services.AddSingleton<NetPeerConfiguration>();
            services.AddScoped<ServerChannel>();
            services.AddScoped<ClientChannel>();
            services.AddTransient<IPipe>();
            services.AddTransient<IUser>();

            // Generate some lookups when default instances get created.
            services.AddSetup<IPeer>((peer, p, c) => p.AddLookupRecursive<IPeer>(c));
            services.AddSetup<NetPeer>((peer, p, c) => p.AddLookupRecursive<NetPeer>(c));

            services.AddSetup<UserList>(GuppyNetworkCoreConstants.ServiceConfigurations.TransientUserList, (users, p, c) =>
            { // Automatically try to add the current user when connecting to a new client. 
                users.TryAdd(p.GetService<IPeer>().CurrentUser);
            }, GuppyCoreConstants.Priorities.Initialize + 1);

            services.AddSetup<INetworkService>((service, p, c) =>
            { // Automatically add any network services into the scoped service list. 
                p.GetService<ServiceList<INetworkService>>().TryAdd(service);
            }, GuppyCoreConstants.Priorities.Initialize + 1);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
