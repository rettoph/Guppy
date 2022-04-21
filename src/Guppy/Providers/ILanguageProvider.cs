using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface ILanguageProvider : IEnumerable<ITextProvider>
    {
        ITextProvider Current { get; }

        void SetCurrentLanguage(string language);
    }
}
