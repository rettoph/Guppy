using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Game.ImGui.Common.Styling.StyleValues;
using Microsoft.Xna.Framework;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Game.ImGui.Common.Serialization.Json.Converters
{
    internal class ImStyleColorValueConverter(Lazy<IResourceService> resourceService) : JsonConverter<ImStyleColorValue>
    {
        private readonly Lazy<IResourceService> _resourceService = resourceService;

        public override ImStyleColorValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = null;
            ImGuiCol col = default;
            ResourceKey<Color> resource = default!;

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
                        resource = ResourceKey<Color>.Get(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new ImStyleColorValue(key, col, _resourceService.Value.Get(resource));
        }

        public override void Write(Utf8JsonWriter writer, ImStyleColorValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
