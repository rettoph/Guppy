using Guppy.Core.Common.Builders;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class GuppyScopeFilterBuilderExtensions
    {
        public static GuppyScopeFilterBuilder RequireSceneHasDebugWindow(this GuppyScopeFilterBuilder filter, bool value)
        {
            return filter.Require(scope => scope.GetSceneHasDebugWindow() == value);
        }

        public static GuppyScopeFilterBuilder RequireSceneHasTerminalWindow(this GuppyScopeFilterBuilder filter, bool value)
        {
            return filter.Require(scope => scope.GetSceneHasTerminalWindow() == value);
        }
    }
}
