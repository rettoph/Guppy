using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Services;
using Guppy.Game.ImGui.Common.Styling.StyleValues;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common.Serialization.Json.Converters
{
    internal class ImStyleColorValueConverter(Lazy<IAssetService> assetService) : JsonConverter<ImStyleColorValue>
    {
        private readonly Lazy<IAssetService> _assetService = assetService;

        public override ImStyleColorValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = null;
            ImGuiCol col = default;
            AssetKey<Color> resource = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(ImStyleValue.Key):
                        key = reader.ReadString();
                        break;

                    case nameof(ImStyleColorValue.Property):
                        col = Enum.Parse<ImGuiCol>(reader.ReadString());
                        break;

                    case nameof(ImStyleColorValue.Color):
                        resource = AssetKey<Color>.Get(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new ImStyleColorValue(key, col, this._assetService.Value.Get(resource));
        }

        public override void Write(Utf8JsonWriter writer, ImStyleColorValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}