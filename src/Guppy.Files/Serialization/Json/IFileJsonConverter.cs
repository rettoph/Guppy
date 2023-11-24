using Guppy.Files.Enums;
using Guppy.Files.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Files.Serialization.Json
{
    public class IFileJsonConverter<T> : JsonConverter<IFile<T>>
        where T : new()
    {
        private Lazy<IFileService> _files;

        public IFileJsonConverter(Lazy<IFileService> files)
        {
            _files = files;
        }

        public override IFile<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.CheckToken(JsonTokenType.StartArray, true);
            reader.Read();

            FileType type = JsonSerializer.Deserialize<FileType>(ref reader, options);
            reader.Read();

            string path = reader.ReadString();

            reader.CheckToken(JsonTokenType.EndArray, true);

            return _files.Value.Get<T>(type, path);
        }

        public override void Write(Utf8JsonWriter writer, IFile<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            JsonSerializer.Serialize(writer, value.Type, options);

            writer.WriteStringValue(value.Path);

            writer.WriteEndArray();
        }
    }
}
