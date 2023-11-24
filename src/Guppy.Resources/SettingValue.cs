using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Guppy.Serialization;
using Guppy.Resources;

namespace Guppy.Resources
{
    internal abstract class SettingValue
    {
        public readonly Setting Setting;

        protected SettingValue(Setting setting)
        {
            this.Setting = setting;
        }
    }
    internal sealed class SettingValue<T> : SettingValue
        where T : notnull
    {
        public new readonly Setting<T> Setting;

        public Ref<T> Value;


        public SettingValue(Setting<T> setting) : base(setting)
        {
            this.Setting = setting;
            this.Value = new Ref<T>(setting.DefaultValue);
        }
    }
}
