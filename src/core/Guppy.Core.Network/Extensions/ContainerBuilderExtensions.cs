﻿using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Groups;
using Guppy.Core.Network.Common.Messages;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Serialization.Json;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Serialization.Json.Converters;
using Guppy.Engine.Common.Autofac;
using LiteNetLib;
using System.Text.Json.Serialization;

namespace Guppy.Core.Network.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreNetworkServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterCoreNetworkServices)))
            {
                return builder;
            }

            builder.RegisterType<NetSerializerService>().As<INetSerializerService>().InstancePerLifetimeScope();
            builder.Register<INetGroup>(ctx => ctx.Resolve<INetScope>().Groups.FirstOrDefault() ?? NotImplementedNetGroup.Instance);
            builder.RegisterType<NetScope>().As<INetScope>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);
            builder.RegisterType<ClientPeer>().As<IClientPeer>().SingleInstance();
            builder.RegisterType<ServerPeer>().As<IServerPeer>().SingleInstance();
            builder.RegisterType<NetMessageService>().As<INetMessageService>().InstancePerLifetimeScope();

            builder.RegisterType<PolymorphicConverter<INetId>>().As<JsonConverter>().SingleInstance();
            builder.RegisterType<ByteNetIdJsonConverter>().As<JsonConverter>().SingleInstance();
            builder.RegisterType<UShortNetIdJsonConverter>().As<JsonConverter>().SingleInstance();
            builder.RegisterType<ClaimJsonConverter>().As<JsonConverter>().SingleInstance();
            builder.RegisterType<ClaimTypeJsonConverter>().As<JsonConverter>().SingleInstance();

            builder.AddNetMessageType<ConnectionRequestData>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);
            builder.AddNetMessageType<ConnectionRequestResponse>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);
            builder.AddNetMessageType<UserAction>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);

            return builder.AddTag(nameof(RegisterCoreNetworkServices));
        }
    }
}
