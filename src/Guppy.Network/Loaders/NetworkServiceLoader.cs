using Autofac;
using Autofac.Configuration;
using Guppy.Common;
using Guppy.Common.Autofac;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common.Providers;
using Guppy.Loaders;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Guppy.Network.Serialization.Json;
using Guppy.Network.Services;
using Guppy.Resources.Serialization.Json.Converters;
using System.Text.Json.Serialization;

namespace Guppy.Network.Loaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<NetSerializerProvider>().As<INetSerializerProvider>().InstancePerLifetimeScope();
            services.RegisterType<NetScope>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.Guppy);
            services.RegisterType<ClientPeer>().SingleInstance();
            services.RegisterType<ServerPeer>().SingleInstance();
            services.RegisterType<NetMessageService>().As<INetMessageService>().InstancePerLifetimeScope();

            services.RegisterType<PolymorphicConverter<INetId>>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ByteNetIdJsonConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<UShortNetIdJsonConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ClaimJsonConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ClaimTypeJsonConverter>().As<JsonConverter>().SingleInstance();

            services.Configure<BrokerConfiguration<IMessage>>((scope, configuration) =>
            {
                configuration.AddMessageAlias<INetOutgoingMessage, INetOutgoingMessage>(true);
            });
        }
    }
}
