using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Resources.Common;
using Guppy.Core.Serialization.Common.Services;

namespace Guppy.Core.Resources.Serialization.Json.Converters
{
    internal class SettingValueConverter(IPolymorphicJsonSerializerService<object> serializer) : JsonConverter<ISettingValue>
    {
        private readonly IPolymorphicJsonSerializerService<object> _serializer = serializer;

        public override ISettingValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string name = string.Empty;
            string description = string.Empty;
            string type = string.Empty;
            JsonElement value = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                switch (propertyName)
                {
                    case nameof(Setting<object>.Name):
                        name = reader.ReadString();
                        break;

                    case nameof(Setting<object>.Description):
                        description = reader.ReadString();
                        break;

                    case nameof(Setting<object>.Type):
                        type = reader.ReadString();
                        break;

                    case nameof(SettingValue<object>.Value):
                        value = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            Type settingValueType = this._serializer.GetType(type);
            object settingValueValue = this._serializer.Deserialize(type, ref value, options, out _);
            ISetting setting = Setting.Get(name, description, settingValueType, settingValueValue);

            ISettingValue settingValue = SettingValue.Create(setting, settingValueValue);

            return settingValue;
        }

        public override void Write(Utf8JsonWriter writer, ISettingValue value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(Setting<object>.Name), value.Setting.Name);
            writer.WriteString(nameof(Setting<object>.Description), value.Setting.Description);
            writer.WriteString(nameof(Setting<object>.Type), value.Type.Name);

            writer.WritePropertyName(nameof(SettingValue<object>.Value));
            JsonSerializer.Serialize(writer, value.Value, options);

            writer.WriteEndObject();
        }
    }
}