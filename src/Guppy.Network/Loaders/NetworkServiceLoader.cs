﻿using Guppy.Common;
using Guppy.Common.DependencyInjection;
using Guppy.Loaders;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Filters;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Guppy.Network.Serialization.Json;
using Guppy.Network.Services;
using Guppy.Resources;
using Guppy.Resources.Serialization.Json.Converters;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Guppy.Network.Loaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INetSerializerProvider, NetSerializerProvider>()
                .AddScoped<NetScope>()
                .AddSingleton<ClientPeer>()
                .AddSingleton<ServerPeer>()
                .AddScoped<INetMessageService, NetMessageService>();

            services.AddSingleton<JsonConverter, PolymorphicJsonConverter<INetId>>()
                    .AddSingleton<JsonConverter, ByteNetIdJsonConverter>()
                    .AddSingleton<JsonConverter, UShortNetIdJsonConverter>()
                    .AddSingleton<JsonConverter, ClaimJsonConverter>()
                    .AddSingleton<JsonConverter, ClaimTypeJsonConverter>();

            services.Configure<BrokerConfiguration>(configuration =>
            {
                configuration.AddMessageAlias<INetOutgoingMessage, INetOutgoingMessage>(true);
            });
        }
    }
}
