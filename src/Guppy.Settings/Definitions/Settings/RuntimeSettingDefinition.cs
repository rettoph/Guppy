using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Definitions.Settings
{
    internal sealed class RuntimeSettingDefinition<T> : SettingDefinition<T>
    {
        public override string? Key { get; }
        public override string? Name { get; }
        public override string? Description { get; }
        public override T DefaultValue { get; }
        public override bool Exportable { get; }
        public override string[] Tags { get; }

        public RuntimeSettingDefinition(
            string? key, 
            string? name, 
            string? description,
            T defaultValue,
            bool exportable, 
            string[] tags)
        {
            this.Key = key;
            this.Name = name;
            this.Description = description;
            this.DefaultValue = defaultValue;
            this.Exportable = exportable;
            this.Tags = tags;
        }
    }
}
