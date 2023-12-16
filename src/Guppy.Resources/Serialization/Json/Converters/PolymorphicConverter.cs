using Guppy.Common.Collections;
using Guppy.Common.Providers;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Guppy.Resources.Serialization.Json.Converters
{
    public sealed class PolymorphicConverter<T> : JsonConverter<T>
        where T : notnull
    {
        public const string TypePropertyKey = "Type";
        public const string ValuePropertyKey = "Value";

        private IPolymorphicJsonSerializer<T> _serializer;

        public PolymorphicConverter(IPolymorphicJsonSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string type = string.Empty;
            JsonElement value = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while(reader.ReadPropertyName(out string? propertyName))
            {
                switch(propertyName)
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

            return _serializer.Deserialize(type, ref value, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            Type implementationType = value.GetType();

            if(implementationType == typeof(T))
            { // Recursion detected
                return;
            }

            writer.WriteStartObject();

            writer.WriteString(TypePropertyKey, _serializer.GetKey(implementationType));

            writer.WritePropertyName(ValuePropertyKey);
            JsonSerializer.Serialize(writer, value, implementationType, options);

            writer.WriteEndObject();
        }
    }
}
