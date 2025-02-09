using Guppy.Core.Resources.Common;

namespace Guppy.Game.MonoGame.Common
{
    public static class GuppyMonoGameSettings
    {
        public static readonly Setting<bool> IsDebugWindowEnabled = Setting<bool>.Get(nameof(IsDebugWindowEnabled), "When true, the debug window will be rendered", false);
        public static readonly Setting<bool> IsTerminalWindowEnabled = Setting<bool>.Get(nameof(IsTerminalWindowEnabled), "When true, the terminal window will be rendered", false);
    }
}