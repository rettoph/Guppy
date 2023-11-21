using Guppy.Common.Providers;
using Guppy.Resources.Definitions;
using Guppy.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal sealed class SettingProvider : ISettingProvider
    {
        private readonly Dictionary<string, ISetting> _settings;

        public SettingProvider(IEnumerable<ISettingDefinition> settings, IJsonSerializer json)
        {
            _settings = settings.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First().Build(json));
        }

        public ISetting<T> Get<T>()
        {
            return this.Get<T>(typeof(T).FullName!);
        }

        public ISetting<T> Get<T>(string key)
        {
            return (ISetting<T>)_settings[key];
        }
    }
}
