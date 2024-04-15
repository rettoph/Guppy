using Guppy.Engine.Serialization;

namespace Guppy.Core.Files
{
    internal class JsonFile<T> : IFile<T>
    {
        private readonly IJsonSerializer _json;
        private T _value;

        public FileLocation Location { get; }
        public FileLocation Source { get; }

        public string Content
        {
            get => _json.Serialize(this.Value);
            set
            {
                T result = _json.Deserialize<T>(value, out bool success);
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

        public JsonFile(FileLocation location, FileLocation source, string content, IJsonSerializer json)
        {
            _value = default!;
            _json = json;

            this.Source = source;
            this.Content = content;
        }

        public JsonFile(StringFile source, IJsonSerializer json) : this(source.Location, source.Source, source.Content, json)
        {

        }
    }
}
