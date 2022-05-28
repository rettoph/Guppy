using Guppy.Network.Definitions;
using Guppy.Network.Definitions.NetMessageFactories;
using Guppy.Network.Definitions.NetSerializers;
using Guppy.Network.Delegates;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddNetSerializer(this IServiceCollection services, Type netSerializerDefinitionType)
        {
            return services.AddSingleton(typeof(NetSerializerDefinition), netSerializerDefinitionType);
        }

        public static IServiceCollection AddNetSerializer<TDefinition>(this IServiceCollection services)
            where TDefinition : NetSerializerDefinition
        {
            return services.AddSingleton<NetSerializerDefinition, TDefinition>();
        }

        public static IServiceCollection AddNetSerializer(this IServiceCollection services, NetSerializerDefinition definition)
        {
            return services.AddSingleton<NetSerializerDefinition>(definition);
        }

        public static IServiceCollection AddNetSerializer<T>(this IServiceCollection services, NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
        {
            return services.AddNetSerializer(new RuntimeNetSerializerDefinition<T>(serialize, deserialize));
        }

        public static IServiceCollection AddNetMessenger(this IServiceCollection services, Type netMessengerDefinitionType)
        {
            return services.AddSingleton(typeof(NetMessageFactoryDefinition), netMessengerDefinitionType);
        }

        public static IServiceCollection AddNetMessenger<TDefinition>(this IServiceCollection services)
            where TDefinition : NetMessageFactoryDefinition
        {
            return services.AddSingleton<NetMessageFactoryDefinition, TDefinition>();
        }

        public static IServiceCollection AddNetMessenger(this IServiceCollection services, NetMessageFactoryDefinition definition)
        {
            return services.AddSingleton<NetMessageFactoryDefinition>(definition);
        }

        public static IServiceCollection AddNetMessenger<T>(this IServiceCollection services, DeliveryMethod deliveryMethod, byte outgoingChannel, int outgoingPriority)
        {
            return services.AddNetMessenger(new RuntimeNetMessageFactoryDefinition<T>(deliveryMethod, outgoingChannel, outgoingPriority));
        }
    }
}
