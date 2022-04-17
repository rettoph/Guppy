namespace Guppy.Settings.Definitions
{
    public abstract class SettingSerializerDefinition
    {
        public abstract Type Type { get; }

        internal SettingSerializerDefinition()
        {

        }

        public abstract SettingSerializer BuildSerializer();
    }

    public abstract class SettingSerializerDefinition<T> : SettingSerializerDefinition
    {
        public override Type Type => typeof(T);

        public abstract string Serialize(T deserialized);
        public abstract T Deserialize(string serialized);

        public override SettingSerializer BuildSerializer()
        {
            return new SettingSerializer<T>(this.Serialize, this.Deserialize);
        }
    }
}
