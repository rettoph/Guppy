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
        public virtual bool Exportable { get; } = false;
        public virtual string[] Tags { get; } = new string[0];

        internal SettingDefinition()
        {

        }

        public abstract ISetting BuildSetting(ISettingSerializerProvider serializers);

        public static string GetKey<T>(string? key)
        {
            return key ?? typeof(T).FullName ?? throw new ArgumentException();
        }
    }

    public abstract class SettingDefinition<T> : SettingDefinition
    {
        public abstract T DefaultValue { get; }

        public override ISetting BuildSetting(ISettingSerializerProvider serializers)
        {
            if(serializers.TryGet<T>(out var serializer))
            {
                return new Setting<T>(
                    key: SettingDefinition.GetKey<T>(this.Key),
                    defaultValue: this.DefaultValue,
                    exportable: this.Exportable,
                    tags: this.Tags,
                    serializer: serializer);
            }

            throw new InvalidOperationException();
        }
    }
}
