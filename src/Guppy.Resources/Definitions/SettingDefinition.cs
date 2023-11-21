using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Serialization;
using Guppy.Serialization;

namespace Guppy.Resources.Definitions
{
    public abstract class SettingDefinition<T> : ISettingDefinition
        where T : notnull
    {
        public abstract string Key { get; }

        public abstract bool Exportable { get; }

        public abstract string[] Tags { get; }

        public Type Type { get; } = typeof(T);

        public abstract object DefaultValue { get; }

        public ISetting Build(IJsonSerializer json)
        {
            return new Setting<T>(
                this.Key, 
                (T)this.DefaultValue,
                this.Exportable, 
                this.Tags,
                json);
        }
    }
}
