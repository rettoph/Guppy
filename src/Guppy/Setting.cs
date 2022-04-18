using Guppy.Providers;

namespace Guppy
{
    public abstract class Setting
    {
        private string? _name;
        private string? _description;
        private ITextProvider _text;

        public readonly Type Type;
        public readonly string Key;
        public string? Name => _text[_name];
        public string? Description => _text[_description];

        internal Setting(Type type, string key, string? name, string? description, ITextProvider text)
        {
            this.Type = type;
            this.Key = key;

            _name = name;
            _description = description;
            _text = text;
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
        public readonly SettingSerializer<T> Serializer;

        public Setting(string key, string? name, string? description, T value, bool exportable, string[] tags, SettingSerializer<T> serializer, ITextProvider text) : base(typeof(T), key, name, description, text)
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
