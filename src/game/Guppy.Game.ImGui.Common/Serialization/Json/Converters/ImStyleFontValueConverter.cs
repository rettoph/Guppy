using Guppy.Game.ImGui.Common.Styling.StyleValues;
using Guppy.Core.Resources.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Game.ImGui.Common.Serialization.Json.Converters
{
    internal class ImStyleFontValueConverter(Lazy<IImGui> gui) : JsonConverter<ImStyleFontValue>
    {
        private readonly Lazy<IImGui> _imgui = gui;

        public override ImStyleFontValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = null;
            int size = default;
            ResourceKey<TrueTypeFont> ttf = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(ImStyleValue.Key):
                        key = reader.ReadString();
                        break;

                    case nameof(ImFontPtr.Size):
                        size = reader.ReadInt32();
                        break;

                    case nameof(ImFontPtr.TTF):
                        ttf = ResourceKey<TrueTypeFont>.Get(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new ImStyleFontValue(key, _imgui.Value.GetFont(ttf, size));
        }

        public override void Write(Utf8JsonWriter writer, ImStyleFontValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
