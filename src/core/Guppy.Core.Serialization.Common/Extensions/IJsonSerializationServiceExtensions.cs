using Guppy.Core.Serialization.Common.Services;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Guppy.Core.Serialization.Common.Extensions
{
    public static class IJsonSerializationServiceExtensions
    {
        public static T? Deserialize<T>(this IJsonSerializationService serializer, string json)
        {
            return serializer.Deserialize<T>(json, out _);
        }
        public static T? Deserialize<T>(this IJsonSerializationService serializer, Stream utf8Json)
        {
            return serializer.Deserialize<T>(utf8Json, out _);
        }
        public static T? Deserialize<T>(this IJsonSerializationService serializer, ref Utf8JsonReader reader)
        {
            return serializer.Deserialize<T>(ref reader, out _);
        }

        public static bool TryDeserialize<T>(this IJsonSerializationService serializer, string json, [MaybeNullWhen(false)] out T value)
        {
            value = serializer.Deserialize<T>(json, out bool success) ?? default!;
            return success;
        }
        public static bool TryDeserialize<T>(this IJsonSerializationService serializer, Stream utf8Json, [MaybeNullWhen(false)] out T value)
        {
            value = serializer.Deserialize<T>(utf8Json, out bool success) ?? default!;
            return success;
        }
        public static bool TryDeserialize<T>(this IJsonSerializationService serializer, ref Utf8JsonReader reader, [MaybeNullWhen(false)] out T value)
        {
            value = serializer.Deserialize<T>(ref reader, out bool success) ?? default!;
            return success;
        }
    }
}
