using Guppy.Game.ImGui.Common.Styling;
using Guppy.Game.ImGui.Common.Styling.StyleValueResources;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Game.ImGui.Common.Serialization.Json.Converters
{
    internal class ImStyleConverter : JsonConverter<ImStyle>
    {
        public override ImStyle? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ImStyle style = new();
            var values = JsonSerializer.Deserialize<List<ImStyleValue>>(ref reader, options) ?? [];
            style.SetValues(values);

            return style;
        }

        public override void Write(Utf8JsonWriter writer, ImStyle value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
