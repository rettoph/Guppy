using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Network.Attributes;
using Guppy.Network.Channels;
using Guppy.Network.Components;
using Guppy.Network.Components.Channels;
using Guppy.Network.Components.Scenes;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Peers;
using Guppy.Network.Pipes;
using Guppy.Network.Security;
using Guppy.Network.Services;
using Guppy.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    public class NetworkServiceLoaders : IServiceLoader
    {
        public void RegisterServices(GuppyServiceCollection services)
        {
            #region Settings Setup
            services.RegisterSetup<Settings>((s, p, c) =>
            { // Configure the default settings...
                s.Set<NetworkAuthorization>(NetworkAuthorization.Slave);
                s.Set<HostType>(HostType.Remote);
            }, -10);

            services.RegisterSetup<ServerPeer>((server, p, c) =>
            {
                var settings = p.GetService<Settings>();
                settings.Set<NetworkAuthorization>(NetworkAuthorization.Master);
            });
            #endregion

            services.RegisterTypeFactory<NetOutgoingMessageService>(p => new NetOutgoingMessageService());
            services.RegisterTypeFactory<ChannelList>(p => new ChannelList());
            services.RegisterTypeFactory<PipeList>(p => new PipeList());
            services.RegisterTypeFactory<UserList>(p => new UserList());
            services.RegisterTypeFactory<ServiceList<INetworkEntity>>(p => new ServiceList<INetworkEntity>());
            services.RegisterTypeFactory<NetworkEntityList>(p => new NetworkEntityList());
            services.RegisterTypeFactory<ServerPeer>(p => new ServerPeer());
            services.RegisterTypeFactory<NetServer>(p => new NetServer(p.GetService<NetPeerConfiguration>()));
            services.RegisterTypeFactory<ClientPeer>(p => new ClientPeer());
            services.RegisterTypeFactory<NetClient>(p => new NetClient(p.GetService<NetPeerConfiguration>()));
            services.RegisterTypeFactory<NetPeerConfiguration>(p => new NetPeerConfiguration("guppy"));
            services.RegisterTypeFactory<ServerChannel>(p => new ServerChannel());
            services.RegisterTypeFactory<ClientChannel>(p => new ClientChannel());
            services.RegisterTypeFactory<IPipe>(p => new Pipe());
            services.RegisterTypeFactory<IUser>(p => new User());

            services.RegisterSingleton<NetOutgoingMessageService>();
            services.RegisterSingleton<ChannelList>();
            services.RegisterScoped<PipeList>();
            services.RegisterSingleton<UserList>();
            services.RegisterTransient(Constants.ServiceConfigurations.TransientUserList);
            services.RegisterTransient<ServiceList<INetworkEntity>>();
            services.RegisterScoped<NetworkEntityList>();
            services.RegisterSingleton<ServerPeer>(baseCacheKey: ServiceConfigurationKey.From<IPeer>());
            services.RegisterSingleton<NetServer>(baseCacheKey: ServiceConfigurationKey.From<NetPeer>());
            services.RegisterSingleton<ClientPeer>(baseCacheKey: ServiceConfigurationKey.From<IPeer>());
            services.RegisterSingleton<NetClient>(baseCacheKey: ServiceConfigurationKey.From<NetPeer>());
            services.RegisterSingleton<NetPeerConfiguration>();
            services.RegisterScoped<ServerChannel>();
            services.RegisterScoped<ClientChannel>();
            services.RegisterTransient<IPipe>();
            services.RegisterTransient<IUser>();

            services.RegisterSetup<UserList>(Constants.ServiceConfigurations.TransientUserList, (users, p, c) =>
            { // Automatically try to add the current user when connecting to a new client.
                // TODO: Investigate what happens if CurrentUser is not yet defined?
                users.TryAdd(p.GetService<IPeer>().CurrentUser);
            }, Guppy.Core.Constants.Priorities.Initialize + 1);

            services.RegisterSetup<INetworkEntity>((service, p, c) =>
            { // Automatically add any network services into the scoped service list. 
                p.GetService<NetworkEntityList>().TryAdd(service);
            }, Guppy.Core.Constants.Priorities.Initialize + 1);

            #region Components
            services.RegisterTypeFactory<ChannelBaseCRUDComponent>(p => new ChannelBaseCRUDComponent());
            services.RegisterTypeFactory<PipeMasterCRUDComponent>(p => new PipeMasterCRUDComponent());

            services.RegisterTransient<ChannelBaseCRUDComponent>();
            services.RegisterTransient<PipeMasterCRUDComponent>();

            services.RegisterComponent<ChannelBaseCRUDComponent, IChannel>();
            services.RegisterComponent<PipeMasterCRUDComponent, IPipe>();

            services.RegisterComponentFilter(
                ServiceConfigurationKey.From(type: typeof(RemoteHostComponent<>)),
                (e, p, t) =>
                {
                    return t.GetCustomAttribute<NetworkAuthorizationRequiredAttribute>().NetworkAuthorization
                        == p.GetService<Settings>().Get<NetworkAuthorization>();
                },
                (cc) =>
                {
                    var hasAttribute = cc.TypeFactory.Type.GetCustomAttribute<NetworkAuthorizationRequiredAttribute>() != default;
                    return hasAttribute;
                });
            #endregion
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
