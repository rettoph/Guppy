using Guppy.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Settings
{
    public class DefaultSettingSerializer<T> : SettingSerializer<T>
        where T : notnull
    {
        protected override T Deserialize(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(ref reader, options) ?? throw new NotImplementedException();
        }

        protected override void Serialize(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
