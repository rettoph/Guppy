using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Serialization.Common.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Guppy.Core.Serialization.Json.Converters
{
    public sealed class PolymorphicConverter<T>(IPolymorphicJsonSerializerService<T> serializer) : JsonConverter<T>
        where T : notnull
    {
        public const string TypePropertyKey = "Type";
        public const string ValuePropertyKey = "Value";

        private readonly IPolymorphicJsonSerializerService<T> _serializer = serializer;

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string type = string.Empty;
            JsonElement value = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                switch (propertyName)
                {
                    case TypePropertyKey:
                        type = reader.ReadString();
                        break;

                    case ValuePropertyKey:
                        value = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return this._serializer.Deserialize(type, ref value, options, out _);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            Type implementationType = value.GetType();

            if (implementationType == typeof(T))
            { // Recursion detected
                return;
            }

            writer.WriteStartObject();

            writer.WriteString(TypePropertyKey, this._serializer.GetKey(implementationType));

            writer.WritePropertyName(ValuePropertyKey);
            JsonSerializer.Serialize(writer, value, implementationType, options);

            writer.WriteEndObject();
        }
    }
}