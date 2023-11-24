using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Settings
{
    public abstract class SettingSerializer
    {
        public abstract Type Type { get; }
        public abstract string Key { get; }

        internal abstract SettingValue Deserialize(string name, ref Utf8JsonReader reader, JsonSerializerOptions options);

        internal abstract void Serialize(Utf8JsonWriter writer, SettingValue settingValue, JsonSerializerOptions options);
    }

    [Service<SettingSerializer>(ServiceLifetime.Singleton, true)]
    public abstract class SettingSerializer<T> : SettingSerializer
        where T : notnull
    {
        public override Type Type => typeof(T);
        public override string Key => this.Type.Name;

        internal override SettingValue Deserialize(string name, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Setting<T> setting = Setting.Define<T>(name, default!);
            SettingValue<T> settingValue = new SettingValue<T>(setting);
            settingValue.Value.Value = this.Deserialize(ref reader, options);

            return settingValue;
        }

        protected abstract T Deserialize(ref Utf8JsonReader reader, JsonSerializerOptions options);

        internal override void Serialize(Utf8JsonWriter writer, SettingValue settingValue, JsonSerializerOptions options)
        {
            if(settingValue is SettingValue<T> casted)
            {
                writer.WritePropertyName($"{this.Key}.{casted.Setting.Name}");
                this.Serialize(writer, casted.Value, options);
            }
        }

        protected abstract void Serialize(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
    }
}
