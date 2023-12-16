using Guppy.Files.Enums;
using Guppy.Serialization;

namespace Guppy.Files
{
    internal class JsonFile<T> : IFile<T>
    {
        private readonly IJsonSerializer _json;
        private T _value;

        public FileType Type { get; }

        public string Path { get; }

        public string FullPath { get; }

        public string Content
        {
            get => _json.Serialize(this.Value);
            set
            {
                T result = _json.Deserialize<T>(value, out bool success) ?? Activator.CreateInstance<T>();
                this.Success = success;

                _value = result;
            }

        }
        public T Value
        {
            get => _value ??= Activator.CreateInstance<T>();
            set => _value = value;
        }

        public bool Success { get; private set; }

        public JsonFile(FileType type, string path, string fullPath, string content, IJsonSerializer json)
        {
            _value = default!;
            _json = json;

            this.Type = type;
            this.Path = path;
            this.FullPath = fullPath;
            this.Content = content;
        }

        public JsonFile(StringFile source, IJsonSerializer json) : this(source.Type, source.Path, source.FullPath, source.Content, json)
        {

        }
    }
}
