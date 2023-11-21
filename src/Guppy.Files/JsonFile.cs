using Guppy.Files.Enums;
using Guppy.Serialization;
using Guppy.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Files
{
    internal class JsonFile<T> : IFile<T>
        where T : new()
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
                T result = _json.Deserialize<T>(value, out bool success) ?? new();
                this.Success = success;

                _value = result;
            }

        }
        public T Value
        {
            get => _value ??= new();
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
