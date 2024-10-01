using Guppy.Core.Network.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Core.Network.Serialization.Json
{
    internal sealed class UShortNetIdJsonConverter : JsonConverter<NetId.UShort>
    {
        public override NetId.UShort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, NetId.UShort value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Value);
        }
    }
}
