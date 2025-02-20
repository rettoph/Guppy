using Guppy.Core.Common.Builders;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class IGuppyContainerBuilderFilterBuilderExtensions
    {
        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequireSceneHasDebugWindow(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> filter, bool value)
        {
            return filter.Require(scope => scope.Variables.GetSceneHasDebugWindow() == value);
        }

        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequireSceneHasTerminalWindow(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> filter, bool value)
        {
            return filter.Require(scope => scope.Variables.GetSceneHasTerminalWindow() == value);
        }
    }
}
