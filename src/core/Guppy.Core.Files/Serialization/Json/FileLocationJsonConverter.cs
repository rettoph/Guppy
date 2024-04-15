using Guppy.Core.Files.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Core.Files.Serialization.Json
{
    internal class FileLocationJsonConverter : JsonConverter<FileLocation>
    {
        public FileLocationJsonConverter()
        {
        }

        public override FileLocation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DirectoryLocation directory = default;
            string name = string.Empty;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                switch (propertyName)
                {
                    case nameof(FileLocation.Directory):
                        directory = JsonSerializer.Deserialize<DirectoryLocation>(ref reader, options);
                        reader.Read();
                        break;
                    case nameof(FileLocation.Name):
                        name = JsonSerializer.Deserialize<string>(ref reader, options) ?? throw new NotImplementedException();
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new FileLocation(directory, name);
        }

        public override void Write(Utf8JsonWriter writer, FileLocation value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(FileLocation.Name), value.Name);

            writer.WritePropertyName(nameof(FileLocation.Directory));
            JsonSerializer.Serialize(writer, value.Directory, options);

            writer.WriteEndObject();
        }
    }
}
