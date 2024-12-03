using Guppy.Core.Resources.Common.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Core.Resources.Serialization.Json.Converters
{
    internal sealed class ResourcePacksConfigurationConverter : JsonConverter<ResourcePackCollectionConfiguration>
    {
        public override ResourcePackCollectionConfiguration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<ResourcePackConfiguration> packs = JsonSerializer.Deserialize<List<ResourcePackConfiguration>>(ref reader, options) ?? [];
            ResourcePackCollectionConfiguration configuration = new(packs);

            return configuration;
        }

        public override void Write(Utf8JsonWriter writer, ResourcePackCollectionConfiguration value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Packs, options);
        }
    }
}
