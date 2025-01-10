using Guppy.Game.MonoGame.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class ISceneConfigurationExtensions
    {
        public static ISceneConfiguration SetSceneHasDebugWindow(this ISceneConfiguration configuration, bool value)
        {
            return configuration.Set(SceneConfigurationKeys.SceneHasDebugWindow, value);
        }

        public static bool GetSceneHasDebugWindow(this ISceneConfiguration configuration)
        {
            return configuration.GetOrDefault(SceneConfigurationKeys.SceneHasDebugWindow, false);
        }

        public static ISceneConfiguration SetSceneHasTerminalWindow(this ISceneConfiguration configuration, bool value)
        {
            return configuration.Set(SceneConfigurationKeys.SceneHasTerminalWindow, value);
        }

        public static bool GetSceneHasTerminalWindow(this ISceneConfiguration configuration)
        {
            return configuration.GetOrDefault(SceneConfigurationKeys.SceneHasTerminalWindow, false);
        }
    }
}