using Guppy.GUI.Styling.StyleValueResources;
using Guppy.Resources;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.GUI.Serialization.Json.Converters
{
    internal class StyleColorValueConverter : JsonConverter<StyleColorValue>
    {
        public override StyleColorValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            GuiCol col = default;
            Resource<Color> resource = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(StyleColorValue.Property):
                        col = Enum.Parse<GuiCol>(reader.ReadString());
                        break;

                    case nameof(StyleColorValue.Resource):
                        resource = Resource.Get<Color>(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new StyleColorValue(col, resource);
        }

        public override void Write(Utf8JsonWriter writer, StyleColorValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
