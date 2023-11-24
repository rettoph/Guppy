using Guppy.Common.Collections;
using Guppy.Resources;
using Guppy.Resources.Serialization.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json.Converters
{
    internal class SettingValueDictionaryConverter : JsonConverter<Dictionary<Setting, SettingValue>>
    {
        private static readonly Regex PropertyPattern = new Regex(@"^(.+?)\.(.+)$");
        private DoubleDictionary<string, Type, SettingSerializer> _serializers;

        public SettingValueDictionaryConverter(IEnumerable<SettingSerializer> serializers)
        {
            _serializers = serializers.ToDoubleDictionary(x => x.Key, x => x.Type);
        }

        public override Dictionary<Setting, SettingValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<Setting, SettingValue> _dict = new Dictionary<Setting, SettingValue>();

            if(reader.CheckToken(JsonTokenType.StartObject, false))
            {
                while (reader.ReadPropertyName(out string? propertyName))
                {
                    var match = PropertyPattern.Match(propertyName);

                    if(match.Success == false)
                    {
                        throw new NotImplementedException();
                    }

                    if (_serializers.TryGet(match.Groups[1].Value, out var serializer) == false)
                    {
                        throw new NotImplementedException();
                    }

                    var settingValue = serializer.Deserialize(match.Groups[2].Value, ref reader, options);
                    reader.Read();

                    _dict[settingValue.Setting] = settingValue;
                }

                reader.CheckToken(JsonTokenType.EndObject, true);
            }

            return _dict;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<Setting, SettingValue> dict, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach(var (setting, value) in dict.OrderBy(x => x.Key.Name))
            {
                if(_serializers.TryGet(setting.Type, out var serializer))
                {
                    serializer.Serialize(writer, value, options);
                }
            }

            writer.WriteEndObject();
        }
    }
}
