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

        Setting ISettingValue.Setting => this.Setting;

        object ISettingValue.Value
        {
            get => this.Value;
            set
            {
                if(value is not T casted)
                {
                    throw new InvalidCastException();
                }

                this.Value = casted;
            }
        }

        public SettingValue(Setting<T> setting) : base(setting.DefaultValue)
        {
            this.Setting = setting;
        }

        public static implicit operator T(SettingValue<T> settingValue)
        {
            return settingValue.Value;
        }
    }
}
