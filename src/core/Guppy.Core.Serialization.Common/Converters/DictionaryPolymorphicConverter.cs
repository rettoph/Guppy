using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Serialization.Common.Services;

namespace Guppy.Core.Serialization.Common.Converters
{
    public sealed class DictionaryPolymorphicConverter<T>(IPolymorphicJsonSerializerService<T> serializer) : JsonConverter<Dictionary<Type, T>>
        where T : notnull
    {
        private readonly IPolymorphicJsonSerializerService<T> _serializer = serializer;

        public override Dictionary<Type, T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<Type, T> result = [];

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                T instance = this._serializer.Deserialize(propertyName, ref reader, options, out Type type);
                reader.Read();

                result.Add(type, instance);
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return result;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<Type, T> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}