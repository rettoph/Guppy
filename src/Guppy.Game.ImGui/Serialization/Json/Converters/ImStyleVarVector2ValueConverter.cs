using Guppy.Game.ImGui.Styling.StyleValueResources;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Game.ImGui.Serialization.Json.Converters
{
    internal class ImStyleVarVector2ValueConverter : JsonConverter<ImStyleVarVector2Value>
    {
        public override ImStyleVarVector2Value? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = null;
            ImGuiStyleVar var = default;
            Vector2 value = default;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(ImStyleValue.Key):
                        key = reader.ReadString();
                        break;

                    case nameof(ImStyleVarVector2Value.Property):
                        var = Enum.Parse<ImGuiStyleVar>(reader.ReadString());
                        break;

                    case nameof(ImStyleVarVector2Value.Value):
                        value = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new ImStyleVarVector2Value(key, var, value);
        }

        public override void Write(Utf8JsonWriter writer, ImStyleVarVector2Value value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
