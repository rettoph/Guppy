using Guppy.Core.Resources;

namespace Guppy.Game.MonoGame
{
    public static class Settings
    {
        public static Setting<bool> IsDebugWindowEnabled = Setting<bool>.Get(nameof(IsDebugWindowEnabled), "When true, the debug window will be rendered", false);
        public static Setting<bool> IsTerminalWindowEnabled = Setting<bool>.Get(nameof(IsTerminalWindowEnabled), "When true, the terminal window will be rendered", false);
    }
}
