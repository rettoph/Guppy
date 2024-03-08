using Guppy.Files;
using Guppy.Resources.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Resources.Serialization.Json.Converters
{
    internal sealed class ResourcePacksConfigurationConverter : JsonConverter<ResourcePacksConfiguration>
    {
        public override ResourcePacksConfiguration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<FileLocation> packs = JsonSerializer.Deserialize<List<FileLocation>>(ref reader, options) ?? new List<FileLocation>();
            ResourcePacksConfiguration configuration = new ResourcePacksConfiguration(packs);

            return configuration;
        }

        public override void Write(Utf8JsonWriter writer, ResourcePacksConfiguration value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Packs, options);
        }
    }
}
