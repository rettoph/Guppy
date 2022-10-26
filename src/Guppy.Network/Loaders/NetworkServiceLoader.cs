using Guppy.Common;
using Guppy.Loaders;
using Guppy.Network.Enums;
using Guppy.Network.Factories;
using Guppy.Network.Factories.NetOutgoingMessageFactories;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Guppy.Resources;
using Guppy.Resources.SettingSerializers;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Network.Loaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        private readonly byte _channelsCount;

        public NetworkServiceLoader(byte channelsCount)
        {
            _channelsCount = channelsCount;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INetSerializerProvider, NetSerializerProvider>()
                .AddSingleton<INetScopeProvider, NetScopeProvider>()
                .AddSingleton<IUserProvider, UserProvider>()
                .AddScoped<NetScope>(p => p.GetRequiredService<INetScopeProvider>().Create(p))
                .AddSingleton<EventBasedNetListener>()
                .AddSingleton<NetManager>(p =>
                {
                    var listener = p.GetRequiredService<EventBasedNetListener>();
                    var manager = new NetManager(listener)
                    {
                        ChannelsCount = _channelsCount,
                        DisconnectTimeout = 60000
                    };

                    return manager;
                })
                .AddFaceted<Peer, ClientPeer>(ServiceLifetime.Singleton)
                .AddFaceted<Peer, ServerPeer>(ServiceLifetime.Singleton)
                .AddSingleton<ISettingTypeSerializer, EnumSettingSerializer<NetAuthorization>>()
                .AddSetting(NetAuthorization.Master, false)
                .AddScoped<ClientNetOutgoingMessageFactory>()
                .AddScoped<ServerNetOutgoingMessageFactory>()
                .AddFilter<INetOutgoingMessageFactory, ClientNetOutgoingMessageFactory>(PeerFilter<ClientPeer>(), 0)
                .AddFilter<INetOutgoingMessageFactory, ClientNetOutgoingMessageFactory>(PeerFilter<ServerPeer>(), 0);
        }

        private Func<IServiceProvider, bool> PeerFilter<T>()
            where T : Peer
        {
            bool Filter(IServiceProvider provider)
            {
                var instance = provider.GetRequiredService<Faceted<Peer>>();
                return instance.Type?.IsAssignableTo(typeof(T)) ?? false;
            }

            return Filter;
        }
    }
}
