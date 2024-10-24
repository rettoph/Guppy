using Guppy.Core.Resources.Common;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Styling;

namespace Guppy.Game.MonoGame.Common
{
    public static class Resources
    {
        public static class TrueTypeFonts
        {
            public static readonly ResourceKey<TrueTypeFont> DiagnosticsFont = ResourceKey<TrueTypeFont>.Get($"{nameof(TrueTypeFont)}.{nameof(DiagnosticsFont)}");
        }

        public static class ImGuiStyles
        {
            public static readonly ResourceKey<ImStyle> DebugWindow = ResourceKey<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(DebugWindow)}");
            public static readonly ResourceKey<ImStyle> ButtonGreen = ResourceKey<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(ButtonGreen)}");
            public static readonly ResourceKey<ImStyle> ButtonRed = ResourceKey<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(ButtonRed)}");
        }
    }
}
