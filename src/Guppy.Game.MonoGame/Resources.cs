using Guppy.Game.ImGui;
using Guppy.Game.ImGui.Styling;
using Guppy.Resources;

namespace Guppy.Game.MonoGame
{
    public static class Resources
    {
        public static class TrueTypeFonts
        {
            public static readonly Resource<TrueTypeFont> DiagnosticsFont = Resource.Get<TrueTypeFont>($"{nameof(TrueTypeFont)}.{nameof(DiagnosticsFont)}");
        }

        public static class ImGuiStyles
        {
            public static readonly Resource<ImStyle> DebugWindow = Resource.Get<ImStyle>($"{nameof(ImStyle)}.{nameof(DebugWindow)}");
            public static readonly Resource<ImStyle> ButtonGreen = Resource.Get<ImStyle>($"{nameof(ImStyle)}.{nameof(ButtonGreen)}");
            public static readonly Resource<ImStyle> ButtonRed = Resource.Get<ImStyle>($"{nameof(ImStyle)}.{nameof(ButtonRed)}");
        }
    }
}
