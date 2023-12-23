using Guppy.Resources;

namespace Guppy.Game.MonoGame
{
    public static class Settings
    {
        public static Setting<bool> IsDebugWindowEnabled = Setting.Get<bool>(nameof(IsDebugWindowEnabled), false);
        public static Setting<bool> IsTerminalWindowEnabled = Setting.Get<bool>(nameof(IsTerminalWindowEnabled), false);
    }
}
