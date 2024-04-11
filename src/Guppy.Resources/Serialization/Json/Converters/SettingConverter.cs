using Guppy.Resources.Services;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Resources.Serialization.Json.Converters
{
    internal class SettingConverter : JsonConverter<ISetting>
    {
        private IPolymorphicJsonSerializerService<object> _serializer;

        public SettingConverter(IPolymorphicJsonSerializerService<object> serializer)
        {
            _serializer = serializer;
        }

        public override ISetting? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                    case nameof(ISetting.Name):
                        name = reader.ReadString();
                        break;

                    case nameof(ISetting.Description):
                        description = reader.ReadString();
                        break;

                    case nameof(ISetting.Type):
                        type = reader.ReadString();
                        break;

                    case nameof(ISetting.Value):
                        value = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            Type implementationType = _serializer.GetImplementationType(type);
            object valueInstance = _serializer.Deserialize(type, ref value, options, out _);
            var getterInfo = typeof(Setting<>).MakeGenericType(implementationType).GetMethod(nameof(Setting<object>.Get), BindingFlags.Static | BindingFlags.Public) ?? throw new NotImplementedException();
            ISetting setting = getterInfo.Invoke(null, [name, description, valueInstance]) as ISetting ?? throw new NotImplementedException();

            setting.Value = valueInstance;

            return setting;
        }

        public override void Write(Utf8JsonWriter writer, ISetting value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(ISetting.Name), value.Name);
            writer.WriteString(nameof(ISetting.Description), value.Description);
            writer.WriteString(nameof(ISetting.Type), value.Type.Name);

            writer.WritePropertyName(nameof(ISetting.Value));
            JsonSerializer.Serialize(writer, value.Value, options);

            writer.WriteEndObject();
        }
    }
}
