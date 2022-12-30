using Guppy.Common.DependencyInjection.Interfaces;
using Guppy.Common.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Filters
{
    public class SettingFilter<TSetting> : SimpleFilter
        where TSetting : notnull
    {
        public readonly string Key;
        public readonly TSetting RequiredValue;

        public SettingFilter(TSetting requiredValue, Type type) : this(typeof(TSetting).FullName!, requiredValue, type)
        {
        }

        public SettingFilter(string key, TSetting requiredValue, Type type) : base(type)
        {
            this.Key = key;
            this.RequiredValue = requiredValue;
        }

        public override bool Invoke(IServiceProvider provider, IServiceConfiguration service)
        {
            var setting = provider.GetSetting<TSetting>(this.Key);
            var result = setting.Value.Equals(this.RequiredValue);

            return result;
        }
    }

    public class SettingFilter<TSetting, TImplementation> : SettingFilter<TSetting>
        where TSetting : notnull
    {
        public SettingFilter(TSetting requiredValue) : base(requiredValue, typeof(TImplementation))
        {
        }

        public SettingFilter(string key, TSetting requiredValue) : base(key, requiredValue, typeof(TImplementation))
        {
        }
    }
}
