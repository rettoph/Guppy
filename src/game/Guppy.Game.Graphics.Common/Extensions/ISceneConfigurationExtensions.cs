using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class ISceneConfigurationExtensions
    {
        public static ISceneConfiguration SetSceneHasGraphicEnabled(this ISceneConfiguration configuration, bool value)
        {
            return configuration.Set(SceneConfigurationKeys.SceneHasGraphicsEnabled, value);
        }

        public static bool GetSceneHasGraphicsEnabled(this ISceneConfiguration configuration)
        {
            return configuration.GetOrDefault(SceneConfigurationKeys.SceneHasGraphicsEnabled, false);
        }

        public static ISceneConfiguration SetSceneHasDebugEnabled(this ISceneConfiguration configuration, bool value)
        {
            return configuration.Set(SceneConfigurationKeys.SceneHasDebugEnabled, value);
        }

        public static bool GetSceneHasDebugEnabled(this ISceneConfiguration configuration)
        {
            return configuration.GetOrDefault(SceneConfigurationKeys.SceneHasDebugEnabled, false);
        }

        public static ISceneConfiguration SetSceneHasTerminalEnabled(this ISceneConfiguration configuration, bool value)
        {
            return configuration.Set(SceneConfigurationKeys.SceneHasTerminalEnabled, value);
        }

        public static bool GetSceneHasTerminalEnabled(this ISceneConfiguration configuration)
        {
            return configuration.GetOrDefault(SceneConfigurationKeys.SceneHasTerminalEnabled, false);
        }
    }
}
