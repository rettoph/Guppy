using Guppy.Common;
using Guppy.Network;
using Guppy.Network.Definitions;
using Guppy.Network.Definitions.NetMessageTypes;
using Guppy.Network.Delegates;
using Guppy.Network.Serialization.NetSerializers;
using LiteNetLib;

namespace Autofac
{
    public static class IServiceCollectionExtensions
    {
        public static void AddNetSerializer(this ContainerBuilder services, Type netSerializerType)
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), netSerializerType);

            services.RegisterType(netSerializerType).As<INetSerializer>().SingleInstance();
        }

        public static void AddNetSerializer<T>(this ContainerBuilder services)
            where T : class, INetSerializer
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), typeof(T));

            services.RegisterType<T>().As<INetSerializer>().SingleInstance();
        }

        public static void AddNetSerializer<T>(this ContainerBuilder services, NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
            where T : notnull
        {
            services.RegisterInstance(new RuntimeNetSerializer<T>(serialize, deserialize)).As<INetSerializer>().SingleInstance();
        }

        public static void AddNetMessageType(this ContainerBuilder services, Type netMessengerDefinitionType)
        {
            services.RegisterType(netMessengerDefinitionType).As<NetMessageTypeDefinition>().SingleInstance();
        }

        public static void AddNetMessageType<TDefinition>(this ContainerBuilder services)
            where TDefinition : NetMessageTypeDefinition
        {
            services.RegisterType<TDefinition>().As<NetMessageTypeDefinition>().SingleInstance();
        }

        public static void AddNetMessageType(this ContainerBuilder services, NetMessageTypeDefinition definition)
        {
            services.RegisterInstance(definition).As<NetMessageTypeDefinition>().SingleInstance();
        }

        public static void AddNetMessageType<T>(this ContainerBuilder services, DeliveryMethod deliveryMethod, byte outgoingChannel)
            where T : notnull
        {
            services.AddNetMessageType(new RuntimeNetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));
        }
    }
}
