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
    internal class StyleVarFloatValueConverter : JsonConverter<StyleVarFloatValue>
    {
        public override StyleVarFloatValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ImGuiStyleVar var = default;
            float value = default;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(StyleVarFloatValue.Property):
                        var = Enum.Parse<ImGuiStyleVar>(reader.ReadString());
                        break;
                    case nameof(StyleVarFloatValue.Value):
                        value = reader.ReadSingle();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new StyleVarFloatValue(var, value);
        }

        public override void Write(Utf8JsonWriter writer, StyleVarFloatValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
