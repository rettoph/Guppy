using Guppy.Network.Definitions;
using Guppy.Network.Definitions.NetMessageTypes;
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

        public static IServiceCollection AddNetMessageType(this IServiceCollection services, Type netMessengerDefinitionType)
        {
            return services.AddSingleton(typeof(NetMessageTypeDefinition), netMessengerDefinitionType);
        }

        public static IServiceCollection AddNetMessageType<TDefinition>(this IServiceCollection services)
            where TDefinition : NetMessageTypeDefinition
        {
            return services.AddSingleton<NetMessageTypeDefinition, TDefinition>();
        }

        public static IServiceCollection AddNetMessageType(this IServiceCollection services, NetMessageTypeDefinition definition)
        {
            return services.AddSingleton<NetMessageTypeDefinition>(definition);
        }

        public static IServiceCollection AddNetMessageType<T>(this IServiceCollection services, DeliveryMethod deliveryMethod, byte outgoingChannel)
        {
            return services.AddNetMessageType(new RuntimeNetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));
        }
    }
}
