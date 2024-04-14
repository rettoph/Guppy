using Guppy.Game.ImGui.Styling.StyleValueResources;
using Guppy.Resources;
using Microsoft.Xna.Framework;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Game.ImGui.Serialization.Json.Converters
{
    internal class ImStyleColorValueConverter : JsonConverter<ImStyleColorValue>
    {
        public override ImStyleColorValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = null;
            ImGuiCol col = default;
            Resource<Color> resource = default!;

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
                        resource = Resource<Color>.Get(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new ImStyleColorValue(key, col, resource);
        }

        public override void Write(Utf8JsonWriter writer, ImStyleColorValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
