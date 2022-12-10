using Guppy.Common;
using Guppy.Common.DependencyInjection;
using Guppy.Loaders;
using Guppy.Network.Enums;
using Guppy.Network.Factories;
using Guppy.Network.Factories.NetOutgoingMessageFactories;
using Guppy.Network.Filters;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Guppy.Network.Serialization.Json;
using Guppy.Resources;
using Guppy.Resources.Serialization.Json.Converters;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

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
                .AddSetting(NetAuthorization.Master, false)
                .ConfigureCollection(manager =>
                {
                    manager.AddScoped<ClientNetOutgoingMessageFactory>()
                        .AddAlias<INetOutgoingMessageFactory>();

                    manager.AddScoped<ServerNetOutgoingMessageFactory>()
                        .AddAlias<INetOutgoingMessageFactory>();
                })
                .AddFilter(new PeerFilter<ClientPeer, ClientNetOutgoingMessageFactory>())
                .AddFilter(new PeerFilter<ServerPeer, ServerNetOutgoingMessageFactory>());

            services.AddSingleton<JsonConverter, PolymorphicJsonConverter<INetId>>()
                    .AddSingleton<JsonConverter, ByteNetIdJsonConverter>()
                    .AddSingleton<JsonConverter, UShortNetIdJsonConverter>()
                    .AddSingleton<JsonConverter, ClaimJsonConverter>()
                    .AddSingleton<JsonConverter, ClaimTypeJsonConverter>();
        }
    }
}
