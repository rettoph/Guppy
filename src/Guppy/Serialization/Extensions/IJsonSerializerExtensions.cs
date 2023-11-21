using Guppy.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Serialization
{
    public static class IJsonSerializerExtensions
    {
        public static T? Deserialize<T>(this IJsonSerializer serializer, string json)
        {
            return serializer.Deserialize<T>(json, out _);
        }
        public static T? Deserialize<T>(this IJsonSerializer serializer, Stream utf8Json)
        {
            return serializer.Deserialize<T>(utf8Json, out _);
        }
        public static T? Deserialize<T>(this IJsonSerializer serializer, ref Utf8JsonReader reader)
        {
            return serializer.Deserialize<T>(ref reader, out _);
        }

        public static bool TryDeserialize<T>(this IJsonSerializer serializer, string json, [MaybeNullWhen(false)] out T value)
        {
            value = serializer.Deserialize<T>(json, out bool success) ?? default!;
            return success;
        }
        public static bool TryDeserialize<T>(this IJsonSerializer serializer, Stream utf8Json, [MaybeNullWhen(false)] out T value)
        {
            value = serializer.Deserialize<T>(utf8Json, out bool success) ?? default!;
            return success;
        }
        public static bool TryDeserialize<T>(this IJsonSerializer serializer, ref Utf8JsonReader reader, [MaybeNullWhen(false)] out T value)
        {
            value = serializer.Deserialize<T>(ref reader, out bool success) ?? default!;
            return success;
        }
    }
}
