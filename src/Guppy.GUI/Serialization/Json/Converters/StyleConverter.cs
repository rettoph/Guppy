using Guppy.GUI.Styling;
using Guppy.GUI.Styling.StyleValueResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.GUI.Serialization.Json.Converters
{
    internal class StyleConverter : JsonConverter<Style>
    {
        public override Style? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Style style = new Style();
            style._values = JsonSerializer.Deserialize<List<StyleValue>>(ref reader, options) ?? style._values;

            return style;
        }

        public override void Write(Utf8JsonWriter writer, Style value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
