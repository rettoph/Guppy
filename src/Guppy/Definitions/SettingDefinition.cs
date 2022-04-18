using Guppy.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Definitions
{
    public abstract class SettingDefinition
    {
        public virtual string? Key { get; } = null;
        public virtual string? Name { get; } = null;
        public virtual string? Description { get; } = null;
        public virtual bool Exportable { get; } = false;
        public virtual string[] Tags { get; } = new string[0];

        internal SettingDefinition()
        {

        }

        public abstract Setting BuildSetting(ISettingSerializerProvider serializers, ITextProvider text);

        public static string GetKey<T>(string? key)
        {
            return key ?? typeof(T).FullName ?? throw new ArgumentException();
        }
    }

    public abstract class SettingDefinition<T> : SettingDefinition
    {
        public abstract T DefaultValue { get; }

        public override Setting BuildSetting(ISettingSerializerProvider serializers, ITextProvider text)
        {
            if(serializers.TryGet<T>(out var serializer))
            {
                return new Setting<T>(
                    key: SettingDefinition.GetKey<T>(this.Key),
                    name: this.Name ?? this.Key ?? typeof(T).Name,
                    description: this.Description,
                    value: this.DefaultValue,
                    exportable: this.Exportable,
                    tags: this.Tags,
                    serializer: serializer,
                    text: text);
            }

            throw new InvalidOperationException();
        }
    }
}
