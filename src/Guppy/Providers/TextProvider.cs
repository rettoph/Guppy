using Guppy.Constants;
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
    internal sealed class TextProvider : ITextProvider
    {
        private ISettingProvider _settings;
        private List<TextDefinition> _definitions;
        private Dictionary<string, LanguageTextProvider> _languages;

        public string CurrentLanguage
        {
            get => _settings.Get<string>(SettingConstants.CurrentLanguage).Value;
            set => _settings.Get<string>(SettingConstants.CurrentLanguage).Value = value;
        }

        public string? this[string? key]
        {
            get
            {
                this.TryGet(key, out string? value);
                return value;
            }
        }

        public TextProvider(ISettingProvider settings, IEnumerable<TextDefinition> texts)
        {
            _settings = settings;
            _definitions = texts.Reverse().Distinct().ToList();
            _languages = new Dictionary<string, LanguageTextProvider>();

            this.SetCurrentLanguage(this.CurrentLanguage);
        }

        public IEnumerable<string> GetLanguages()
        {
            return _languages.Keys;
        }

        public void SetCurrentLanguage(string language)
        {
            if(!_languages.ContainsKey(language))
            {
                _languages[language] = new LanguageTextProvider(language, _definitions);
            }

            this.CurrentLanguage = language;
        }

        public bool TryGet(string? key, out string? value)
        {
            if(_languages[this.CurrentLanguage].TryGet(key, out value))
            {
                return true;
            }

            value = key;
            return false;
        }

        public IEnumerator<TextDefinition> GetEnumerator()
        {
            return _definitions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
