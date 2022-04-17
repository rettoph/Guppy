using Guppy.Settings.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Providers
{
    internal sealed class SettingProvider : ISettingProvider
    {
        private readonly ISettingSerializerProvider _serializers;
        private readonly Dictionary<string, Setting> _settings;

        public Setting this[string key] => _settings[key];

        public SettingProvider(ISettingSerializerProvider serializers, IEnumerable<SettingDefinition> definitions)
        {
            _serializers = serializers;
            _settings = new Dictionary<string, Setting>(definitions.Count());

            foreach(SettingDefinition definition in definitions)
            {
                var setting = definition.BuildSetting(_serializers);
                _settings.Add(setting.Key, setting);
            }
        }

        public Setting<T> Get<T>()
        {
            return this.Get<T>(SettingDefinition.GetKey<T>(null));
        }

        public Setting<T> Get<T>(string key)
        {
            if (_settings[key] is Setting<T> casted)
            {
                return casted;
            }

            throw new ArgumentException();
        }

        public IEnumerator<Setting> GetEnumerator()
        {
            return _settings.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _settings.Values.GetEnumerator();
        }
    }
}
