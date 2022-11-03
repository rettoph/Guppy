using Guppy.Common;
using Guppy.Network;
using Guppy.Network.Definitions;
using Guppy.Network.Definitions.NetMessageTypes;
using Guppy.Network.Delegates;
using Guppy.Network.NetSerializers;
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
        public static IServiceCollection AddNetSerializer(this IServiceCollection services, Type netSerializerType)
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), netSerializerType);

            return services.AddScoped(typeof(INetSerializer), netSerializerType);
        }

        public static IServiceCollection AddNetSerializer<T>(this IServiceCollection services)
            where T : class, INetSerializer
        {
            ThrowIf.Type.IsNotGenericTypeImplementation(typeof(INetSerializer<>), typeof(T));

            return services.AddScoped<INetSerializer, T>();
        }

        public static IServiceCollection AddNetSerializer<T>(this IServiceCollection services, NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize)
            where T : notnull
        {
            return services.AddScoped<INetSerializer>(p => new RuntimeNetSerializer<T>(serialize, deserialize));
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
            where T : notnull
        {
            return services.AddNetMessageType(new RuntimeNetMessageTypeDefinition<T>(deliveryMethod, outgoingChannel));
        }
    }
}
