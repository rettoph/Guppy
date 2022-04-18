using Guppy.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface ITextProvider : IEnumerable<TextDefinition>
    {
        public string? this[string? key] { get; }
        public string CurrentLanguage { get; }

        public bool TryGet(string? key, out string? value);

        public void SetCurrentLanguage(string language);

        public IEnumerable<string> GetLanguages();
    }
}
