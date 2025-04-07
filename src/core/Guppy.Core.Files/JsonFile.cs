using Guppy.Core.Files.Common;
using Guppy.Core.Serialization.Common.Services;

namespace Guppy.Core.Files
{
    internal class JsonFile<T> : IFile<T>
    {
        private readonly IJsonSerializationService _json;
        private T _value;

        public FilePath Location { get; }
        public FilePath Source { get; }

        public string Content
        {
            get => this._json.Serialize(this.Value);
            set
            {
                T result = this._json.Deserialize<T>(value, out bool success);
                this.Success = success;

                this._value = result;
            }

        }
        public T Value
        {
            get => this._value ??= Activator.CreateInstance<T>();
            set => this._value = value;
        }

        public bool Success { get; private set; }

        public JsonFile(FilePath location, FilePath source, string content, IJsonSerializationService json)
        {
            this._value = default!;
            this._json = json;

            this.Location = location;
            this.Source = source;
            this.Content = content;
        }

        public JsonFile(StringFile source, IJsonSerializationService json) : this(source.Location, source.Source, source.Content, json)
        {

        }
    }
}