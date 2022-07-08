using Guppy.Resources.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal sealed class SettingProvider : ISettingProvider
    {
        private Dictionary<string, ISetting> _settings;

        public SettingProvider(IEnumerable<ISettingDefinition> settings, IEnumerable<ISettingSerializer> serializers)
        {
            _settings = settings.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First().Build(serializers));
        }

        public ISetting<T> Get<T>()
        {
            return this.Get<T>(nameof(T));
        }

        public ISetting<T> Get<T>(string key)
        {
            return (ISetting<T>)_settings[key];
        }
    }
}
