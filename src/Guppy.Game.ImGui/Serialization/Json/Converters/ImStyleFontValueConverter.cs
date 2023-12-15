using Guppy.Game.ImGui.Styling.StyleValueResources;
using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui.Serialization.Json.Converters
{
    internal class ImStyleFontValueConverter : JsonConverter<ImStyleFontValue>
    {
        private readonly Lazy<IImGui> _imgui;

        public ImStyleFontValueConverter(Lazy<IImGui> gui)
        {
            _imgui = gui;
        }

        public override ImStyleFontValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = null;
            int size = default;
            Resource<TrueTypeFont> ttf = default!;

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
                        ttf = Resources.Resource.Get<TrueTypeFont>(reader.ReadString());
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
