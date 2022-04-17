using Guppy.Settings.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Loaders.Definitions
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

        public abstract SettingDescriptor BuildDescriptor();
    }

    public abstract class SettingDefinition<T> : SettingDefinition
    {
        public abstract T? DefaultValue { get; }

        public override SettingDescriptor BuildDescriptor()
        {
            return SettingDescriptor.Create(
                key: this.Key ?? typeof(T).FullName ?? throw new InvalidOperationException(),
                name: this.Name,
                description: this.Description,
                defaultValue: this.DefaultValue,
                exportable: this.Exportable,
                tags: this.Tags);
        }
    }
}
