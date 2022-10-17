using Guppy.Loaders;
using Guppy.Network.Enums;
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
            services.AddSingleton<INetSerializerProvider, NetSerializerProvider>()
                .AddSingleton<INetDatumProvider, NetDatumProvider>()
                .AddSingleton<INetMessageProvider, NetMessageProvider>()
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
                .AddSetting(NetAuthorization.Master, false);
        }
    }
}
