using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Guppy.Serialization;
using Guppy.Resources;
using Guppy.Common;

namespace Guppy.Resources
{
    public sealed class SettingValue<T> : Ref<T>, ISettingValue
        where T : notnull
    {
        public readonly Setting<T> Setting;
        public readonly T DefaultValue;

        Setting ISettingValue.Setting => this.Setting;

        public SettingValue(Setting<T> setting, T defaultValue) : base(defaultValue)
        {
            this.Setting = setting;
            this.DefaultValue = defaultValue;
        }

        public static implicit operator T(SettingValue<T> settingValue)
        {
            return settingValue.Value;
        }

        void ISettingValue.SetValue(ISettingValue value)
        {
            if(value is SettingValue<T> casted)
            {
                this.Value = casted.Value;
                return;
            }

            throw new NotImplementedException();
        }
    }
}
