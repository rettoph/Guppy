using Guppy.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class LanguageTextProvider
    {
        private Dictionary<string, string?> _texts;
        private string _name;

        public LanguageTextProvider(string name, IEnumerable<TextDefinition> texts)
        {
            _name = name;
            _texts = texts.ToDictionary(x => x.Key, x => x.DefaultValue);
        }

        public bool TryGet(string? key, out string? value)
        {
            if(key is null)
            {
                value = null;
                return false;
            }

            return _texts.TryGetValue(key, out value);
        }
    }
}
