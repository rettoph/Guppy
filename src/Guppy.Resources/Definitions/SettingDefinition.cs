using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Definitions
{
    public abstract class SettingDefinition<T> : ISettingDefinition
        where T : notnull
    {
        public abstract string Key { get; }

        public abstract bool Exportable { get; }

        public abstract string[] Tags { get; }

        public Type Type => typeof(T);

        public abstract object DefaultValue { get; }

        public ISetting Build(IEnumerable<ISettingTypeSerializer> serializers)
        {
            return new Setting<T>(
                this.Key, 
                (T)this.DefaultValue, 
                (ISettingSerializer<T>)serializers.First(x => x.Type == typeof(T)), 
                this.Exportable, 
                this.Tags);
        }
    }
}
