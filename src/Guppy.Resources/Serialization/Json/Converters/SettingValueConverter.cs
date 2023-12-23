using Guppy.Resources.Providers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Resources.Serialization.Json.Converters
{
    internal class SettingValueConverter : JsonConverter<ISettingValue>
    {
        private IPolymorphicJsonSerializer<object> _serializer;

        public SettingValueConverter(IPolymorphicJsonSerializer<object> serializer)
        {
            _serializer = serializer;
        }

        public override ISettingValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string name = string.Empty;
            string type = string.Empty;
            JsonElement value = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                switch (propertyName)
                {
                    case nameof(Setting.Name):
                        name = reader.ReadString();
                        break;

                    case nameof(Setting.Description):
                        reader.ReadString();
                        break;

                    case nameof(Setting.Type):
                        type = reader.ReadString();
                        break;

                    case nameof(ISettingValue.Value):
                        value = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            Type implementationType = _serializer.GetImplementationType(type);
            Setting setting = Setting.Get(implementationType, name);

            Type settingValueType = typeof(SettingValue<>).MakeGenericType(implementationType);
            ISettingValue settingValue = (ISettingValue)Activator.CreateInstance(settingValueType, setting)!;

            settingValue.Value = _serializer.Deserialize(type, ref value, options);

            return settingValue;
        }

        public override void Write(Utf8JsonWriter writer, ISettingValue value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(Setting.Name), value.Setting.Name);

            if (value.Setting.Description is not null)
            {
                writer.WriteString(nameof(Setting.Description), value.Setting.Description);
            }

            writer.WriteString(nameof(Setting.Type), value.Setting.Type.Name);

            writer.WritePropertyName(nameof(ISettingValue.Value));
            JsonSerializer.Serialize(writer, value.Value, options);

            writer.WriteEndObject();
        }
    }
}
