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
        public override StyleFontValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int size = default;
            Resource<TrueTypeFont> resource = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(StyleFontValue.Size):
                        size = reader.ReadInt32();
                        break;

                    case nameof(StyleFontValue.Resource):
                        resource = Resources.Resource.Get<TrueTypeFont>(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new StyleFontValue(resource, size);
        }

        public override void Write(Utf8JsonWriter writer, StyleFontValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
