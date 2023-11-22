using Guppy.GUI.Styling.StyleValueResources;
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
    internal class StyleVarVector2ValueConverter : JsonConverter<StyleVarVector2Value>
    {
        public override StyleVarVector2Value? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            GuiStyleVar var = default;
            Vector2 value = default;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(StyleVarVector2Value.Property):
                        var = Enum.Parse<GuiStyleVar>(reader.ReadString());
                        break;
                    case nameof(StyleVarVector2Value.Value):
                        value = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new StyleVarVector2Value(var, value);
        }

        public override void Write(Utf8JsonWriter writer, StyleVarVector2Value value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
