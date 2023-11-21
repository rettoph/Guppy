using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Guppy.Serialization;

namespace Guppy.Resources
{
    internal sealed class Setting<T> : ISetting<T>
    {
        private IJsonSerializer _json;
        public T Value { get; set; }

        public T DefaultValue { get; }

        public string Key { get; }

        public bool Exportable { get; }

        public string[] Tags { get; }

        public Type Type => typeof(T);

        public Setting(string key, T defaultValue, bool exportable, string[] tags, IJsonSerializer json)
        {
            _json = json;

            this.Key = key;
            this.Value = defaultValue;
            this.DefaultValue = defaultValue;
            this.Exportable = exportable;
            this.Tags = tags;
        }

        void ISetting.Import(string value)
        {
            this.Value = _json.Deserialize<T>(value) ?? throw new Exception();
        }

        string ISetting.Export()
        {
            return _json.Serialize<T>(this.Value);
        }
    }
}
