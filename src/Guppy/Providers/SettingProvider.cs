using Guppy.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class SettingProvider : ResourceProvider<ISetting>, ISettingProvider
    {
        private readonly ISettingSerializerProvider _serializers;
        private readonly Dictionary<string, ISetting> _settings;

        public SettingProvider(ISettingSerializerProvider serializers, IEnumerable<SettingDefinition> definitions)
        {
            _serializers = serializers;
            _settings = definitions.Select(x => x.BuildSetting(_serializers)).ToDictionary();
        }

        public override bool TryGet(string key, [MaybeNullWhen(false)] out ISetting resource)
        {
            return _settings.TryGetValue(key, out resource);
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

        public bool TryGet<T>([MaybeNullWhen(false)] out Setting<T> setting)
        {
            return this.TryGet<T>(SettingDefinition.GetKey<T>(null), out setting);
        }

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out Setting<T> setting)
        {
            if (_settings[key] is Setting<T> casted)
            {
                setting = casted;
                return true;
            }

            setting = null;
            return false;
        }

        public override IEnumerator<ISetting> GetEnumerator()
        {
            return _settings.Values.GetEnumerator();
        }
    }
}
