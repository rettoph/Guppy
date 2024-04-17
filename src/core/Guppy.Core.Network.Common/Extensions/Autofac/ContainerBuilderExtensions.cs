using Guppy.Core.Common;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Contexts;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Delegates;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Serialization.NetSerializers;
using LiteNetLib;

namespace Autofac
{
    public static class ContainerBuilderExtensions
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
            services.AddNetMessageType(new NetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));
        }

        /// <summary>
        /// Configure scoped <see cref="Guppy.Core.Network.Common.INetScope{T}"/> instance.
        /// Traditionally this is done on scope creation with a custom builder action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="peerType"></param>
        /// <param name="groupId"></param>
        public static void RegisterNetScope<T>(this ContainerBuilder services, PeerType peerType, byte groupId)
        {
            services.RegisterInstance<NetScopeContext<T>>(new NetScopeContext<T>(peerType, groupId)).SingleInstance();
            services.Register<INetScope>(ctc => ctc.Resolve<INetScope<T>>()).InstancePerLifetimeScope();
        }
    }
}
