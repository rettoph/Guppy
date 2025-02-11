using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Contexts;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Delegates;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Serialization.NetSerializers;
using LiteNetLib;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterPeerTypeFilter(this IGuppyScopeBuilder builder, PeerTypeEnum peerType, Action<IGuppyScopeBuilder> build)
        {
            return builder.Filter(filter => filter.RequirePeerType(peerType), build);
        }

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

        public static IGuppyScopeBuilder RegisterNetMessageType<T, TNetSerializer>(this IGuppyScopeBuilder builder, DeliveryMethod deliveryMethod, byte outgoingChannel)
            where T : notnull
            where TNetSerializer : class, INetSerializer<T>
        {
            builder.RegisterNetMessageType(new NetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));
            builder.RegisterNetSerializer<TNetSerializer>();

            return builder;
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
            builder.AddScopeVariable(GuppyNetworkVariables.Scope.PeerType.Create(peerType));
            builder.RegisterInstance<NetScopeContext<T>>(new NetScopeContext<T>(peerType, groupId)).SingleInstance();
            builder.Register<INetScope>(ctc => ctc.Resolve<INetScope<T>>()).InstancePerLifetimeScope();

            return builder;
        }
    }
}