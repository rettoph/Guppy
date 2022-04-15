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
        private Dictionary<string, Setting> _settings;

        public Setting this[string key] => _settings[key];

        public SettingProvider(IEnumerable<Setting> settings)
        {
            _settings = settings.ToDictionary(
                keySelector: x => x.Key,
                elementSelector: x => x);
        }

        public Setting<T> Get<T>()
        {
            return this.Get<T>(typeof(T).FullName ?? throw new InvalidOperationException());
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
