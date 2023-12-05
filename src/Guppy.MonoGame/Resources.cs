using Guppy.Game.ImGui;
using Guppy.Game.ImGui.Styling;
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
            public static readonly Resource<TrueTypeFont> DiagnosticsFont = Resource.Get<TrueTypeFont>($"{nameof(TrueTypeFont)}.{nameof(DiagnosticsFont)}");
        }

        public static class Styles
        {
            public static readonly Resource<ImStyle> DebugWindow = Resource.Get<ImStyle>($"{nameof(ImStyle)}.{nameof(DebugWindow)}");
        }
    }
}
