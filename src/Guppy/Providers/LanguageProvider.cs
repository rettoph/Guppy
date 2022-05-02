using Guppy.Constants;
using Guppy.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class LanguageProvider : ILanguageProvider
    {
        private ISettingProvider _settings;
        private IEnumerable<TextDefinition> _definitions;
        private Dictionary<string, ITextProvider> _languages;

        public ITextProvider Current { get; private set; }

        public LanguageProvider(ISettingProvider settings, IEnumerable<TextDefinition> definitions)
        {
            _settings = settings;
            _definitions = definitions;
            _languages = new Dictionary<string, ITextProvider>();

            this.Current = default!;
            this.SetCurrentLanguage(_settings.Get<string>(SettingConstants.CurrentLanguage).Value);
        }

        public void SetCurrentLanguage(string language)
        {
            if(!_languages.TryGetValue(language, out ITextProvider? text))
            {
                text = new TextProvider(language, _definitions);
                _languages.Add(language, text);
            }

            this.Current = text;
            _settings.Get<string>(SettingConstants.CurrentLanguage).Value = language;
        }

        public IEnumerator<ITextProvider> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
