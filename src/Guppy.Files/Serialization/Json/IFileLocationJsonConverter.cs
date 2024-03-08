using Guppy.Files.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Files.Serialization.Json
{
    internal class IFileLocationJsonConverter : JsonConverter<FileLocation>
    {
        public IFileLocationJsonConverter()
        {
        }

        public override FileLocation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.CheckToken(JsonTokenType.StartArray, true);
            reader.Read();

            FileType type = JsonSerializer.Deserialize<FileType>(ref reader, options);
            reader.Read();

            string path = reader.ReadString();

            reader.CheckToken(JsonTokenType.EndArray, true);

            return new FileLocation(type, path);
        }

        public override void Write(Utf8JsonWriter writer, FileLocation value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            JsonSerializer.Serialize(writer, value.Type, options);

            writer.WriteStringValue(value.Path);

            writer.WriteEndArray();
        }
    }
}
