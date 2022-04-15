using Guppy.Settings.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings
{
    public abstract class Setting
    {
        public readonly Type Type;
        public readonly string Key;
        public readonly string Name;
        public readonly string Description;

        internal Setting(Type type, string key, string name, string description)
        {
            this.Type = type;
            this.Key = key;
            this.Name = name;
            this.Description = description;
        }

        public abstract bool TrySetValue(object value);
        public abstract object? GetValue();

        public abstract string SerializeValue();
        public abstract void DeserializeValue(string serializedValue);
    }

    public sealed class Setting<T> : Setting
    {
        public T Value;

        public readonly bool Exportable;
        public readonly string[] Tags;
        public readonly SettingSerializerDescriptor<T> Serializer;

        public Setting(string key, string name, string description, T value, bool exportable, string[] tags, SettingSerializerDescriptor<T> serializer) : base(typeof(T), key, name, description)
        {
            this.Value = value;
            this.Exportable = exportable;
            this.Tags = tags;
            this.Serializer = serializer;
        }

        public override bool TrySetValue(object value)
        {
            if(value is T casted)
            {
                this.Value = casted;
                return true;
            }

            throw new ArgumentException($"{nameof(TrySetValue)}<{this.Type.GetPrettyName()}>:{nameof(TrySetValue)} - Invalid type '{value.GetType().GetPrettyName()}'.", nameof(value));
        }

        public override object? GetValue()
        {
            return this.Value;
        }

        public override string SerializeValue()
        {
            return this.Serializer.Serialize(this.Value);
        }

        public override void DeserializeValue(string serializedValue)
        {
            this.Value = this.Serializer.Deserialize(serializedValue);
        }
    }
}
