using Guppy.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class TextProvider : ResourceProvider<Text>, ITextProvider
    {
        private Dictionary<string, Text> _texts;

        public readonly string Language;

        public TextProvider(string language, IEnumerable<TextDefinition> definitions)
        {
            _texts = definitions.Select(x => x.BuildText()).ToDictionary();

            this.Language = language;
        }

        public override bool TryGet(string key, [MaybeNullWhen(false)] out Text resource)
        {
            return _texts.TryGetValue(key, out resource);
        }

        public override IEnumerator<Text> GetEnumerator()
        {
            return _texts.Values.GetEnumerator();
        }
    }
}
