using Guppy.Game.ImGui;
using Guppy.Game.ImGui.Styling;
using Guppy.Core.Resources;

namespace Guppy.Game.MonoGame
{
    public static class Resources
    {
        public static class TrueTypeFonts
        {
            public static readonly Resource<TrueTypeFont> DiagnosticsFont = Resource<TrueTypeFont>.Get($"{nameof(TrueTypeFont)}.{nameof(DiagnosticsFont)}");
        }

        public static class ImGuiStyles
        {
            public static readonly Resource<ImStyle> DebugWindow = Resource<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(DebugWindow)}");
            public static readonly Resource<ImStyle> ButtonGreen = Resource<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(ButtonGreen)}");
            public static readonly Resource<ImStyle> ButtonRed = Resource<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(ButtonRed)}");
        }
    }
}
