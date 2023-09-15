using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json.Converters
{
    internal class RawResourceValuesConverter : JsonConverter<RawResourceValues>
    {
        public override RawResourceValues? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.String:
                    return new RawResourceValues(reader.GetString() ?? string.Empty);
                case JsonTokenType.StartArray:
                    return new RawResourceValues(JsonSerializer.Deserialize<string[]>(ref reader, options) ?? Array.Empty<string>());
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Write(Utf8JsonWriter writer, RawResourceValues value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
