﻿using Guppy.Core.Network.Common.Services;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Providers
{
    public static class INetSerializerProviderExtensions
    {
        public static void Serialize<T>(this INetSerializerService serializers, NetDataWriter writer, bool sign, in T instance)
            where T : notnull
        {
            var serializer = serializers.Get<T>();

            SignIfShould(sign, serializer.Id, writer);

            serializer.Serialize(writer, in instance);
        }

        public static void Serialize(this INetSerializerService serializers, NetDataWriter writer, in object instance)
        {
            var serializer = serializers.Get(instance.GetType());

            SignIfShould(true, serializer.Id, writer);

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
            var serializer = serializers.Get<T>();

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
            var id = NetId.Byte.Read(reader);
            var serializer = serializers.Get(id);

            return serializer.Deserialize(reader);
        }

        private static void SignIfShould(bool should, INetId id, NetDataWriter writer)
        {
            if (should)
            {
                id.Write(writer);
            }
        }
    }
}
