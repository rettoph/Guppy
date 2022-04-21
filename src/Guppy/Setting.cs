using Guppy.Providers;

namespace Guppy
{
    public sealed class Setting<T> : Resource<T>, ISetting
    {
        public readonly bool Exportable;
        public readonly string[] Tags;
        public readonly SettingSerializer<T> Serializer;

        public Setting(string key, T defaultValue, bool exportable, string[] tags, SettingSerializer<T> serializer) : base(key, defaultValue)
        {
            this.Value = defaultValue;
            this.Exportable = exportable;
            this.Tags = tags;
            this.Serializer = serializer;
        }

        public override string Export()
        {
            return this.Serializer.Serialize(this.Value);
        }

        public override void Import(string serializedValue)
        {
            this.Value = this.Serializer.Deserialize(serializedValue);
        }
    }
}
