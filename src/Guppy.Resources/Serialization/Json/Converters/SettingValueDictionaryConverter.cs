using Guppy.Common.Collections;
using Guppy.Resources;
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
    internal class SettingValueDictionaryConverter : JsonConverter<Dictionary<Setting, ISettingValue>>
    {
        public override Dictionary<Setting, ISettingValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<Setting, ISettingValue> _dict = new Dictionary<Setting, ISettingValue>();

            if(reader.CheckToken(JsonTokenType.StartObject, false))
            {
                while (reader.ReadPropertyName(out string? propertyName))
                {
                    if(Setting.TryGet(propertyName, out Setting? setting) == false)
                    {
                        throw new NotImplementedException();
                    }

                    var settingValue = setting.Deserialize(propertyName, ref reader, options);
                    reader.Read();

                    _dict[settingValue.Setting] = settingValue;
                }

                reader.CheckToken(JsonTokenType.EndObject, true);
            }

            return _dict;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<Setting, ISettingValue> dict, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach(var (setting, value) in dict.OrderBy(x => x.Key.Name))
            {
                setting.Serialize(writer, value, options);
            }

            writer.WriteEndObject();
        }
    }
}
