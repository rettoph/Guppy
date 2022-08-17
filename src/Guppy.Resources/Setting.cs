using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    internal sealed class Setting<T> : ISetting<T>
    {
        public T Value { get; set; }

        public T DefaultValue { get; }

        public ISettingSerializer<T> Serializer { get; }

        public string Key { get; }

        public bool Exportable { get; }

        public string[] Tags { get; }

        public Type Type => typeof(T);

        ISettingTypeSerializer ISetting.Serializer => this.Serializer;

        public Setting(string key, T defaultValue, ISettingSerializer<T> serializer, bool exportable, string[] tags)
        {
            this.Value = defaultValue;
            this.DefaultValue = defaultValue;
            this.Serializer = serializer;
            this.Key = key;
            this.Exportable = exportable;
            this.Tags = tags;
        }
    }
}
