using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Delegates;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Serialization.NetSerializers;
using LiteNetLib;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterNetSerializer(this IGuppyRootBuilder builder, Type netSerializerType)
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), netSerializerType);

            builder.RegisterType(netSerializerType).As<INetSerializer>().SingleInstance();

            return builder;
        }

        public static IGuppyRootBuilder RegisterNetSerializer<T>(this IGuppyRootBuilder builder)
            where T : class, INetSerializer
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), typeof(T));

            builder.RegisterType<T>().As<INetSerializer>().SingleInstance();

            return builder;
        }

        public static IGuppyRootBuilder RegisterNetSerializer<T>(this IGuppyRootBuilder builder, NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
            where T : notnull
        {
            builder.RegisterInstance(new RuntimeNetSerializer<T>(serialize, deserialize)).As<INetSerializer>().SingleInstance();

            return builder;
        }

        public static IGuppyRootBuilder RegisterNetMessageType(this IGuppyRootBuilder builder, Type netMessengerDefinitionType)
        {
            builder.RegisterType(netMessengerDefinitionType).As<NetMessageTypeDefinition>().SingleInstance();

            return builder;
        }

        public static IGuppyRootBuilder RegisterNetMessageType<TDefinition>(this IGuppyRootBuilder builder)
            where TDefinition : NetMessageTypeDefinition
        {
            builder.RegisterType<TDefinition>().As<NetMessageTypeDefinition>().SingleInstance();

            return builder;
        }

        public static IGuppyRootBuilder RegisterNetMessageType(this IGuppyRootBuilder builder, NetMessageTypeDefinition definition)
        {
            builder.RegisterInstance(definition).As<NetMessageTypeDefinition>().SingleInstance();

            return builder;
        }

        public static IGuppyRootBuilder RegisterNetMessageType<T>(this IGuppyRootBuilder builder, DeliveryMethod deliveryMethod, byte outgoingChannel)
            where T : notnull
        {
            return builder.RegisterNetMessageType(new NetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));
        }

        public static IGuppyRootBuilder RegisterNetMessageType<T, TNetSerializer>(this IGuppyRootBuilder builder, DeliveryMethod deliveryMethod, byte outgoingChannel)
            where T : notnull
            where TNetSerializer : class, INetSerializer<T>
        {
            builder.RegisterNetMessageType(new NetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));
            builder.RegisterNetSerializer<TNetSerializer>();

            return builder;
        }
    }
}
