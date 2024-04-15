using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Common.Loaders;
using Guppy.Core.Network.Constants;
using Guppy.Core.Network.Groups;
using Guppy.Core.Network.Messages;
using Guppy.Core.Network.Peers;
using Guppy.Core.Network.Serialization.Json;
using Guppy.Core.Network.Services;
using Guppy.Core.Resources.Serialization.Json.Converters;
using LiteNetLib;
using System.Text.Json.Serialization;

namespace Guppy.Core.Network.Loaders
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
