using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class ISceneConfigurationExtensions
    {
        public static ISceneConfiguration SetSceneHasGraphicEnabled(this ISceneConfiguration configuration, bool value)
        {
            return configuration.Set(GraphicsSceneConfigurationKeys.SceneHasGraphicsEnabled, value);
        }

        public static bool GetSceneHasGraphicsEnabled(this ISceneConfiguration configuration)
        {
            return configuration.GetOrDefault(GraphicsSceneConfigurationKeys.SceneHasGraphicsEnabled, true);
        }
    }
}
