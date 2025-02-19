﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Identity.Enums;

namespace Guppy.Core.Network.Serialization.Json
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
            ClaimAccessibilityEnum accessibility = default!;
            DateTime createdAt = default!;
            object? value = default!;


            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadProperty(out Properties property))
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
                        accessibility = JsonSerializer.Deserialize<ClaimAccessibilityEnum>(ref reader, options);
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

            Claim claim = type.Create(key, value ?? throw new JsonException(), accessibility, createdAt);

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

            object? value = claim.GetValue();
            if (value is not null)
            {
                writer.WritePropertyName(Properties.Value);
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }

            writer.WriteEndObject();
        }
    }
}