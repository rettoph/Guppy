﻿using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public static class INetSerializerProviderExtensions
    {
        public static void Serialize<T>(this INetSerializerProvider serializers, NetDataWriter writer, bool sign, in T instance)
            where T : notnull
        {
            var serializer = serializers.Get<T>();

            SignIfShould(sign, serializer.Id, writer);

            serializer.Serialize(writer, serializers, in instance);
        }

        public static void Serialize(this INetSerializerProvider serializers, NetDataWriter writer, in object instance)
        {
            var serializer = serializers.Get(instance.GetType());

            SignIfShould(true, serializer.Id, writer);

            serializer.Serialize(writer, serializers, in instance);
        }

        /// <summary>
        /// Deserialize an unsigned object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializers"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this INetSerializerProvider serializers, NetDataReader reader)
            where T : notnull
        {
            var serializer = serializers.Get<T>();

            return serializer.Deserialize(reader, serializers);
        }

        /// <summary>
        /// Deserialize a signed object
        /// </summary>
        /// <param name="serializers"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static object Deserialize(this INetSerializerProvider serializers, NetDataReader reader)
        {
            var id = NetId.Byte.Read(reader);
            var serializer = serializers.Get(id);

            return serializer.Deserialize(reader, serializers);
        }

        private static void SignIfShould(bool shouldSign, INetId id, NetDataWriter writer)
        {
            if(shouldSign)
            {
                id.Write(writer);
            }
        }
    }
}
