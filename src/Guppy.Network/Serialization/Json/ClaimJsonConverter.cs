using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Network.Serialization.Json
{
    internal sealed class ClaimJsonConverter : JsonConverter<Claim>
    {
        private enum Properties
        {
            Type,
            Key,
            Accessibility,
            CreatedAt,
            Value
        }

        public override Claim? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ClaimType? type = default!;
            string key = default!;
            ClaimAccessibility accessibility = default!;
            DateTime createdAt = default!;
            object? value = default!;


            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while(reader.ReadProperty(out Properties property))
            {
                switch (property)
                {
                    case Properties.Type:
                        string typeName = reader.ReadString();
                        type = ClaimType.Get(typeName);
                        break;
                    case Properties.Key:
                        key = reader.ReadString();
                        break;
                    case Properties.Accessibility:
                        accessibility = JsonSerializer.Deserialize<ClaimAccessibility>(ref reader, options);
                        reader.Read();
                        break;
                    case Properties.CreatedAt:
                        createdAt = JsonSerializer.Deserialize<DateTime>(ref reader, options);
                        reader.Read();
                        break;
                    case Properties.Value:
                        value = JsonSerializer.Deserialize(ref reader, type?.Type ?? throw new JsonException(), options);
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            Claim claim = type.Create(key, value ?? throw new JsonException(), accessibility);
            claim.CreatedAt = createdAt;

            return claim;
        }

        public override void Write(Utf8JsonWriter writer, Claim claim, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(Properties.Type, claim.Type.Name);

            writer.WriteString(Properties.Key, claim.Key);

            writer.WritePropertyName(Properties.Accessibility);
            JsonSerializer.Serialize(writer, claim.Accessibility, options);

            writer.WritePropertyName(Properties.CreatedAt);
            JsonSerializer.Serialize(writer, claim.CreatedAt, options);

            var value = claim.GetValue();
            if(value is not null)
            {
                writer.WritePropertyName(Properties.Value);
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }

            writer.WriteEndObject();
        }
    }
}
