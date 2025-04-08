using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Files.Common;

namespace Guppy.Core.Files.Serialization.Json
{
    internal class FilePathJsonConverter : JsonConverter<FilePath>
    {
        public FilePathJsonConverter()
        {
        }

        public override FilePath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DirectoryPath directory = default;
            string name = string.Empty;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                switch (propertyName)
                {
                    case nameof(FilePath.DirectoryPath):
                        directory = JsonSerializer.Deserialize<DirectoryPath>(ref reader, options);
                        reader.Read();
                        break;
                    case nameof(FilePath.FileName):
                        name = JsonSerializer.Deserialize<string>(ref reader, options) ?? throw new NotImplementedException();
                        reader.Read();
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new FilePath(directory, name);
        }

        public override void Write(Utf8JsonWriter writer, FilePath value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(FilePath.FileName), value.FileName);

            writer.WritePropertyName(nameof(FilePath.DirectoryPath));
            JsonSerializer.Serialize(writer, value.DirectoryPath, options);

            writer.WriteEndObject();
        }
    }
}