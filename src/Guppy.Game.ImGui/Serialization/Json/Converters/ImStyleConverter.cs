using Guppy.Game.ImGui.Styling;
using Guppy.Game.ImGui.Styling.StyleValueResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui.Serialization.Json.Converters
{
    internal class ImStyleConverter : JsonConverter<ImStyle>
    {
        public override ImStyle? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ImStyle style = new ImStyle();
            style._values = JsonSerializer.Deserialize<List<ImStyleValue>>(ref reader, options) ?? style._values;

            return style;
        }

        public override void Write(Utf8JsonWriter writer, ImStyle value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
