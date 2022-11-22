using Guppy.Network.Identity.Claims;
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
        private const string ValuePropertyKey = "Value";

        public override Claim? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Claim claim, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(Claim.Type), claim.Type.Name);

            writer.WriteString(nameof(Claim.Key), claim.Key);

            writer.WritePropertyName(nameof(Claim.Accessiblity));
            JsonSerializer.Serialize(writer, claim.Accessiblity, options);

            writer.WritePropertyName(nameof(Claim.CreatedAt));
            JsonSerializer.Serialize(writer, claim.CreatedAt, options);

            var value = claim.GetValue();
            if(value is not null)
            {
                writer.WritePropertyName(ValuePropertyKey);
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }

            writer.WriteEndObject();
        }
    }
}
