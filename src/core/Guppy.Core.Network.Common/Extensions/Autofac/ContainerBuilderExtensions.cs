using Guppy.Core.Common;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Contexts;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Delegates;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Serialization.NetSerializers;
using Guppy.Core.StateMachine.Common.Extensions;
using LiteNetLib;

namespace Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterNetSerializer(this ContainerBuilder builder, Type netSerializerType)
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), netSerializerType);

            builder.RegisterType(netSerializerType).As<INetSerializer>().SingleInstance();
        }

        public static void RegisterNetSerializer<T>(this ContainerBuilder builder)
            where T : class, INetSerializer
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), typeof(T));

            builder.RegisterType<T>().As<INetSerializer>().SingleInstance();
        }

        public static void RegisterNetSerializer<T>(this ContainerBuilder builder, NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
            where T : notnull => builder.RegisterInstance(new RuntimeNetSerializer<T>(serialize, deserialize)).As<INetSerializer>().SingleInstance();

        public static void RegisterNetMessageType(this ContainerBuilder builder, Type netMessengerDefinitionType) => builder.RegisterType(netMessengerDefinitionType).As<NetMessageTypeDefinition>().SingleInstance();

        public static void RegisterNetMessageType<TDefinition>(this ContainerBuilder builder)
            where TDefinition : NetMessageTypeDefinition => builder.RegisterType<TDefinition>().As<NetMessageTypeDefinition>().SingleInstance();

        public static void RegisterNetMessageType(this ContainerBuilder builder, NetMessageTypeDefinition definition) => builder.RegisterInstance(definition).As<NetMessageTypeDefinition>().SingleInstance();

        public static void RegisterNetMessageType<T>(this ContainerBuilder builder, DeliveryMethod deliveryMethod, byte outgoingChannel)
            where T : notnull => builder.RegisterNetMessageType(new NetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));

        /// <summary>
        /// Configure scoped <see cref="Guppy.Core.Network.Common.INetScope{T}"/> instance.
        /// Traditionally this is done on scope creation with a custom builder action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="peerType"></param>
        /// <param name="groupId"></param>
        public static void RegisterNetScope<T>(this ContainerBuilder builder, PeerTypeEnum peerType, byte groupId)
        {
            builder.RegisterInstance<NetScopeContext<T>>(new NetScopeContext<T>(peerType, groupId)).SingleInstance();
            builder.Register<INetScope>(ctc => ctc.Resolve<INetScope<T>>()).InstancePerLifetimeScope();
        }

        /// <summary>
        /// Register a peer type service filter
        /// </summary>
        /// <typeparam name="TService">The service type to be filtered</typeparam>
        /// <param name="builder"></param>
        /// <param name="value">The PeerType required for the service to be valid</param>
        /// <returns></returns>
        public static ContainerBuilder RegisterPeerTypeFilter<TService>(this ContainerBuilder builder, PeerTypeEnum value)
            where TService : class => builder.RegisterStateFilter<TService, PeerTypeEnum>(value);


        /// <summary>
        /// Register a peer type service filter
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceType">he service type to be filtered</param>
        /// <param name="value">The PeerType required for the service to be valid</param>
        /// <returns></returns>
        public static ContainerBuilder RegisterPeerTypeFilter(this ContainerBuilder builder, Type serviceType, PeerTypeEnum value) => builder.RegisterStateFilter<PeerTypeEnum>(serviceType, value);
    }
}