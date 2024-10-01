using Guppy.Core.Files.Common.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Core.Files.Common.Serialization.Json
{
    public class IFileJsonConverter<T>(Lazy<IFileService> files) : JsonConverter<IFile<T>>
        where T : new()
    {
        private Lazy<IFileService> _files = files;

        public override IFile<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            FileLocation location = JsonSerializer.Deserialize<FileLocation>(ref reader, options);

            return _files.Value.Get<T>(location);
        }

        public override void Write(Utf8JsonWriter writer, IFile<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Location, options);
        }
    }
}
