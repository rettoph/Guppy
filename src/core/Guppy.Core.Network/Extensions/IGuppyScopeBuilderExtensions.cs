using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Extensions;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Messages;
using Guppy.Core.Network.Peers;
using Guppy.Core.Network.Serialization.Json;
using Guppy.Core.Network.Serialization.NetSerializers;
using Guppy.Core.Network.Services;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Core.Serialization.Json.Converters;
using LiteNetLib;

namespace Guppy.Core.Network.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCoreNetworkServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreNetworkServices), builder =>
            {
                builder.RegisterType<NetSerializerService>().As<INetSerializerService>().InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(NetScope<>)).As(typeof(INetScope<>)).InstancePerLifetimeScope();
                builder.RegisterType<ClientPeer>().As<IClientPeer>().SingleInstance();
                builder.RegisterType<ServerPeer>().As<IServerPeer>().SingleInstance();
                builder.RegisterType<NetMessageService>().As<INetMessageService>().InstancePerLifetimeScope();

                builder.RegisterJsonConverter<PolymorphicConverter<INetId>>();
                builder.RegisterJsonConverter<ByteNetIdJsonConverter>();
                builder.RegisterJsonConverter<UShortNetIdJsonConverter>();
                builder.RegisterJsonConverter<ClaimJsonConverter>();
                builder.RegisterJsonConverter<ClaimTypeJsonConverter>();

                builder.RegisterPolymorphicJsonType<NetId.Byte, INetId>(nameof(NetId.Byte));
                builder.RegisterPolymorphicJsonType<NetId.UShort, INetId>(nameof(NetId.UShort));

                builder.RegisterNetMessageType<ConnectionRequestData>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);
                builder.RegisterNetMessageType<ConnectionRequestResponse>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);
                builder.RegisterNetMessageType<UserAction>(DeliveryMethod.ReliableOrdered, PeerConstants.OutgoingChannel);

                builder.RegisterNetSerializer<ConnectionRequestDataNetSerializer>();
                builder.RegisterNetSerializer<ConnectionRequestResponseNetSerializer>();
                builder.RegisterNetSerializer<UserActionNetSerializer>();
                builder.RegisterNetSerializer<UserDtoNetSerializer>();
            });
        }
    }
}