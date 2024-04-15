using Guppy.Core.Resources.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Core.Resources.Serialization.Json.Converters
{
    internal sealed class ResourcePacksConfigurationConverter : JsonConverter<ResourcePacksConfiguration>
    {
        public override ResourcePacksConfiguration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<ResourcePackConfiguration> packs = JsonSerializer.Deserialize<List<ResourcePackConfiguration>>(ref reader, options) ?? new List<ResourcePackConfiguration>();
            ResourcePacksConfiguration configuration = new ResourcePacksConfiguration(packs);

            return configuration;
        }

        public override void Write(Utf8JsonWriter writer, ResourcePacksConfiguration value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Packs, options);
        }
    }
}
