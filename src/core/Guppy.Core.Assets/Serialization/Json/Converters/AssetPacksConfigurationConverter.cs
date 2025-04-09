using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Assets.Common.Configuration;

namespace Guppy.Core.Assets.Serialization.Json.Converters
{
    internal sealed class AssetPacksConfigurationConverter : JsonConverter<AssetPackCollectionConfiguration>
    {
        public override AssetPackCollectionConfiguration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<AssetPackConfiguration> packs = JsonSerializer.Deserialize<List<AssetPackConfiguration>>(ref reader, options) ?? [];
            AssetPackCollectionConfiguration configuration = new(packs);

            return configuration;
        }

        public override void Write(Utf8JsonWriter writer, AssetPackCollectionConfiguration value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Packs, options);
        }
    }
}