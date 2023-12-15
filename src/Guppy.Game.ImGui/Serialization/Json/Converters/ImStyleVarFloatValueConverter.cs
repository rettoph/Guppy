using Guppy.Game.ImGui.Styling.StyleValueResources;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui.Serialization.Json.Converters
{
    internal class ImStyleVarFloatValueConverter : JsonConverter<ImStyleVarFloatValue>
    {
        public override ImStyleVarFloatValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = null;
            ImGuiStyleVar var = default;
            float value = default;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(ImStyleValue.Key):
                        key = reader.ReadString();
                        break;

                    case nameof(ImStyleVarFloatValue.Property):
                        var = Enum.Parse<ImGuiStyleVar>(reader.ReadString());
                        break;

                    case nameof(ImStyleVarFloatValue.Value):
                        value = reader.ReadSingle();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new ImStyleVarFloatValue(key, var, value);
        }

        public override void Write(Utf8JsonWriter writer, ImStyleVarFloatValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
