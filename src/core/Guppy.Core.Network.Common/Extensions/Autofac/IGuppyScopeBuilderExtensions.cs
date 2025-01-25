using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Network.Common.Contexts;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Delegates;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Serialization.NetSerializers;
using Guppy.Core.StateMachine.Common.Extensions;
using LiteNetLib;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterNetSerializer(this IGuppyScopeBuilder builder, Type netSerializerType)
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), netSerializerType);

            builder.RegisterType(netSerializerType).As<INetSerializer>().SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterNetSerializer<T>(this IGuppyScopeBuilder builder)
            where T : class, INetSerializer
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), typeof(T));

            builder.RegisterType<T>().As<INetSerializer>().SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterNetSerializer<T>(this IGuppyScopeBuilder builder, NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
            where T : notnull
        {
            builder.RegisterInstance(new RuntimeNetSerializer<T>(serialize, deserialize)).As<INetSerializer>().SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterNetMessageType(this IGuppyScopeBuilder builder, Type netMessengerDefinitionType)
        {
            builder.RegisterType(netMessengerDefinitionType).As<NetMessageTypeDefinition>().SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterNetMessageType<TDefinition>(this IGuppyScopeBuilder builder)
            where TDefinition : NetMessageTypeDefinition
        {
            builder.RegisterType<TDefinition>().As<NetMessageTypeDefinition>().SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterNetMessageType(this IGuppyScopeBuilder builder, NetMessageTypeDefinition definition)
        {
            builder.RegisterInstance(definition).As<NetMessageTypeDefinition>().SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterNetMessageType<T>(this IGuppyScopeBuilder builder, DeliveryMethod deliveryMethod, byte outgoingChannel)
            where T : notnull
        {
            return builder.RegisterNetMessageType(new NetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));
        }

        /// <summary>
        /// Configure scoped <see cref="Guppy.Core.Network.Common.INetScope{T}"/> instance.
        /// Traditionally this is done on scope creation with a custom builder action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="peerType"></param>
        /// <param name="groupId"></param>
        public static IGuppyScopeBuilder RegisterNetScope<T>(this IGuppyScopeBuilder builder, PeerTypeEnum peerType, byte groupId)
        {
            builder.RegisterInstance<NetScopeContext<T>>(new NetScopeContext<T>(peerType, groupId)).SingleInstance();
            builder.Register<INetScope>(ctc => ctc.Resolve<INetScope<T>>()).InstancePerLifetimeScope();

            return builder;
        }

        /// <summary>
        /// Register a peer type service filter
        /// </summary>
        /// <typeparam name="TService">The service type to be filtered</typeparam>
        /// <param name="builder"></param>
        /// <param name="value">The PeerType required for the service to be valid</param>
        /// <returns></returns>
        public static IGuppyScopeBuilder RegisterPeerTypeFilter<TService>(this IGuppyScopeBuilder builder, PeerTypeEnum value)
            where TService : class
        {
            return builder.RegisterStateFilter<TService, PeerTypeEnum>(value);
        }


        /// <summary>
        /// Register a peer type service filter
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceType">he service type to be filtered</param>
        /// <param name="value">The PeerType required for the service to be valid</param>
        /// <returns></returns>
        public static IGuppyScopeBuilder RegisterPeerTypeFilter(this IGuppyScopeBuilder builder, Type serviceType, PeerTypeEnum value)
        {
            return builder.RegisterStateFilter<PeerTypeEnum>(serviceType, value);
        }
    }
}