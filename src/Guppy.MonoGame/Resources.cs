using Guppy.GUI;
using Guppy.GUI.Styling;
using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public static class Resources
    {
        public static class TrueTypeFonts
        {
            public static readonly Resource<TrueTypeFont> DiagnosticsFont = Guppy.Resources.Resource.Get<TrueTypeFont>($"{nameof(GUI.TrueTypeFont)}.{nameof(DiagnosticsFont)}");
        }

        public static class Styles
        {
            public static readonly Resource<Style> DebugWindow = Guppy.Resources.Resource.Get<Style>($"{nameof(GUI.Styling.Style)}.{nameof(DebugWindow)}");
        }
    }
}
