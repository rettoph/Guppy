using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public static class Resources
    {
        public static class TrueTypeFonts
        {
            public static readonly Resource<TrueTypeFont> DiagnosticsFont = Resource.Get<TrueTypeFont>($"{nameof(TrueTypeFont)}.{nameof(DiagnosticsFont)}");
        }
    }
}
