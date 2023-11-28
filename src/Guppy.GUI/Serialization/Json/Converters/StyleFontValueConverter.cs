using Guppy.GUI.Styling.StyleValueResources;
using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.GUI.Serialization.Json.Converters
{
    internal class StyleFontValueConverter : JsonConverter<StyleFontValue>
    {
        private readonly Lazy<IGui> _gui;

        public StyleFontValueConverter(Lazy<IGui> gui)
        {
            _gui = gui;
        }

        public override StyleFontValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int size = default;
            Resource<TrueTypeFont> ttf = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(GuiFontPtr.Size):
                        size = reader.ReadInt32();
                        break;

                    case nameof(GuiFontPtr.TTF):
                        ttf = Resources.Resource.Get<TrueTypeFont>(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new StyleFontValue(_gui.Value.GetFont(ttf, size));
        }

        public override void Write(Utf8JsonWriter writer, StyleFontValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
