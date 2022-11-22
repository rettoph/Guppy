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
    internal sealed class ClaimTypeJsonConverter : JsonConverter<ClaimType>
    {
        public override ClaimType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ClaimType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}
