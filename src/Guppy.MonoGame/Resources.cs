using Guppy.GUI;
using Guppy.GUI.Styling;
using Guppy.Resources;
using Microsoft.Xna.Framework;
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

        public static class Colors
        {
            public static readonly Resource<Color> TerminalDefault = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalDefault)}");
            public static readonly Resource<Color> TerminalFatal = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalFatal)}");
            public static readonly Resource<Color> TerminalError = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalError)}");
            public static readonly Resource<Color> TerminalWarning = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalWarning)}");
            public static readonly Resource<Color> TerminalInformation = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalInformation)}");
            public static readonly Resource<Color> TerminalDebug = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalDebug)}");
            public static readonly Resource<Color> TerminalVerbose = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalVerbose)}");
        }
    }
}
