using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Enums;

namespace Guppy.Core.Files.Serialization.Json
{
    internal sealed class DirectoryLocationJsonConverter : JsonConverter<DirectoryLocation>
    {
        public DirectoryLocationJsonConverter()
        {
        }

        public override DirectoryLocation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DirectoryTypeEnum type = DirectoryTypeEnum.AppData;
            string path = string.Empty;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                switch (propertyName)
                {
                    case nameof(DirectoryLocation.Type):
                        type = JsonSerializer.Deserialize<DirectoryTypeEnum>(ref reader, options);
                        reader.Read();
                        break;
                    case nameof(DirectoryLocation.Path):
                        path = JsonSerializer.Deserialize<string>(ref reader, options) ?? throw new NotImplementedException();
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new DirectoryLocation(type, path);
        }

        public override void Write(Utf8JsonWriter writer, DirectoryLocation value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(DirectoryLocation.Type));
            JsonSerializer.Serialize(writer, value.Type, options);

            writer.WriteString(nameof(DirectoryLocation.Path), value.Path);

            writer.WriteEndObject();
        }
    }
}