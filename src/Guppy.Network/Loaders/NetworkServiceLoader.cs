using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Loaders;
using Guppy.Network.Constants;
using Guppy.Network.Groups;
using Guppy.Network.Messages;
using Guppy.Network.Peers;
using Guppy.Network.Serialization.Json;
using Guppy.Network.Services;
using Guppy.Resources.Serialization.Json.Converters;
using LiteNetLib;
using System.Text.Json.Serialization;

namespace Guppy.Network.Loaders
{
    [AutoLoad]
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<NetSerializerService>().As<INetSerializerService>().InstancePerLifetimeScope();
            services.Register<INetGroup>(ctx => ctx.Resolve<INetScope>().Groups.FirstOrDefault() ?? NotImplementedNetGroup.Instance);
            services.RegisterType<NetScope>().As<INetScope>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);
            services.RegisterType<ClientPeer>().As<IClientPeer>().SingleInstance();
            services.RegisterType<ServerPeer>().As<IServerPeer>().SingleInstance();
            services.RegisterType<NetMessageService>().As<INetMessageService>().InstancePerLifetimeScope();

            services.RegisterType<PolymorphicConverter<INetId>>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ByteNetIdJsonConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<UShortNetIdJsonConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ClaimJsonConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ClaimTypeJsonConverter>().As<JsonConverter>().SingleInstance();

            services.AddNetMessageType<ConnectionRequestData>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);
            services.AddNetMessageType<ConnectionRequestResponse>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);
            services.AddNetMessageType<UserAction>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);
        }
    }
}
