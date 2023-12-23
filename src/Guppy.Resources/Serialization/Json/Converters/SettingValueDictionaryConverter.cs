using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Resources.Serialization.Json.Converters
{
    internal class SettingValueDictionaryConverter : JsonConverter<Dictionary<Setting, ISettingValue>>
    {
        public override Dictionary<Setting, ISettingValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<Setting, ISettingValue> _dict = new Dictionary<Setting, ISettingValue>();

            if (reader.CheckToken(JsonTokenType.StartArray, false))
            {
                ISettingValue[]? values = JsonSerializer.Deserialize<ISettingValue[]>(ref reader, options);

                if (values is not null)
                {
                    foreach (ISettingValue value in values)
                    {
                        _dict.Add(value.Setting, value);
                    }
                }

                reader.CheckToken(JsonTokenType.EndArray, true);
            }

            return _dict;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<Setting, ISettingValue> dict, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var (setting, value) in dict.OrderBy(x => x.Key.Name))
            {
                JsonSerializer.Serialize(writer, value!, options);
            }

            writer.WriteEndArray();
        }
    }
}
