using Guppy.Core.Assets.Common;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Styling;

namespace Guppy.Game.MonoGame.Common
{
    public static class Assets
    {
        public static class TrueTypeFonts
        {
            public static readonly AssetKey<TrueTypeFont> DiagnosticsFont = AssetKey<TrueTypeFont>.Get($"{nameof(TrueTypeFont)}.{nameof(DiagnosticsFont)}");
        }

        public static class ImGuiStyles
        {
            public static readonly AssetKey<ImStyle> DebugWindow = AssetKey<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(DebugWindow)}");
            public static readonly AssetKey<ImStyle> ButtonGreen = AssetKey<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(ButtonGreen)}");
            public static readonly AssetKey<ImStyle> ButtonRed = AssetKey<ImStyle>.Get($"{nameof(ImStyle)}.{nameof(ButtonRed)}");
        }
    }
}