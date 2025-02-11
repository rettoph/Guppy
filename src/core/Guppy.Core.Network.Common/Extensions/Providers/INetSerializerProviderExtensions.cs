using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Services;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Providers
{
    public static class INetSerializerProviderExtensions
    {
        public static void Serialize<T>(this INetSerializerService serializers, NetDataWriter writer, bool sign, in T instance)
            where T : notnull
        {
            INetSerializer<T> serializer = serializers.Get<T>();

            if (sign == true)
            {
                serializer.Id.Write(writer); // Sign the message type to the message
            }

            serializer.Serialize(writer, in instance);
        }

        public static void Serialize(this INetSerializerService serializers, NetDataWriter writer, in object instance)
        {
            INetSerializer serializer = serializers.Get(instance.GetType());

            serializer.Id.Write(writer); // Sign the message type to the message
            serializer.Serialize(writer, in instance);
        }

        /// <summary>
        /// Deserialize an unsigned object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializers"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this INetSerializerService serializers, NetDataReader reader)
            where T : notnull
        {
            INetSerializer<T> serializer = serializers.Get<T>();

            return serializer.Deserialize(reader);
        }

        /// <summary>
        /// Deserialize a signed object
        /// </summary>
        /// <param name="serializers"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static object Deserialize(this INetSerializerService serializers, NetDataReader reader)
        {
            INetId<byte> id = NetId.Byte.Read(reader);
            INetSerializer serializer = serializers.Get(id);

            return serializer.Deserialize(reader);
        }
    }
}