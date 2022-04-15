using Guppy.Settings.Loaders.Collections;

namespace Guppy.Settings.Loaders.Descriptors
{
    public abstract class SettingDescriptor
    {
        public readonly Type Type;
        public readonly string Key;
        public readonly string? Name;
        public readonly string? Description;

        protected SettingDescriptor(Type type, string key, string? name, string? description)
        {
            this.Type = type;
            this.Key = key;
            this.Name = name;
            this.Description = description;
        }

        public static SettingDescriptor Create<T>(string key, string? name, string? description, T? defaultValue, bool exportable, params string[] tags)
        {
            return new SettingDescriptor<T>(key, name, description, defaultValue, exportable, tags);
        }
        public static SettingDescriptor Create<T>(string? name, string? description, T? defaultValue, bool exportable, params string[] tags)
        {
            return new SettingDescriptor<T>(nameof(T), name, description, defaultValue, exportable, tags);
        }

        public abstract Setting BuildSetting(ISettingSerializerCollection serializerCollection);
    }

    internal sealed class SettingDescriptor<T> : SettingDescriptor
    {
        public readonly T? DefaultValue;
        public readonly bool Exportable;
        public readonly string[] Tags;

        public SettingDescriptor(string key, string? name, string? description, T? defaultValue, bool exportable, string[] tags) : base(typeof(T), key, name, description)
        {
            this.DefaultValue = defaultValue;
            this.Exportable = exportable;
            this.Tags = tags;
        }

        public override Setting BuildSetting(ISettingSerializerCollection serializerCollection)
        {
            SettingSerializerDescriptor<T> serializer = serializerCollection
                .Where(x => x is SettingSerializerDescriptor<T>)?
                .Last() as SettingSerializerDescriptor<T> ?? throw new InvalidOperationException($"{nameof(SettingDescriptor)}<{typeof(T).GetPrettyName()}>:{nameof(Create)} - No suitable {nameof(SettingSerializerDescriptor)} found.");

            return new Setting<T>(
                key: this.Key,
                name: this.Name,
                description: this.Description,
                value: this.DefaultValue,
                exportable: this.Exportable,
                tags: this.Tags,
                serializer: serializer);
        }
    }
}