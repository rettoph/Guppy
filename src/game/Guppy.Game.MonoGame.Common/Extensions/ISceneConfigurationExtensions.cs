using Guppy.Game.MonoGame.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class ISceneConfigurationExtensions
    {
        public static ISceneConfiguration SetSceneHasDebugWindow(this ISceneConfiguration configuration, bool value) => configuration.Set(SceneConfigurationKeys.SceneHasDebugWindow, value);

        public static bool GetSceneHasDebugWindow(this ISceneConfiguration configuration) => configuration.GetOrDefault(SceneConfigurationKeys.SceneHasDebugWindow, false);

        public static ISceneConfiguration SetSceneHasTerminalWindow(this ISceneConfiguration configuration, bool value) => configuration.Set(SceneConfigurationKeys.SceneHasTerminalWindow, value);

        public static bool GetSceneHasTerminalWindow(this ISceneConfiguration configuration) => configuration.GetOrDefault(SceneConfigurationKeys.SceneHasTerminalWindow, false);
    }
}