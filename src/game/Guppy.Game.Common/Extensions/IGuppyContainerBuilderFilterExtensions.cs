using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Common.Constants;

namespace Guppy.Game.Common.Extensions
{
    public static class IGuppyContainerBuilderFilterExtensions
    {
        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequireScene(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> filter,
            Type? sceneType)
        {
            if (sceneType is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);
            }

            return filter.RequireScopeVariable(GuppyGameVariables.Scope.SceneType.Create(sceneType));
        }

        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequireScene<TScene>(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> filter)
        {
            return filter.RequireScopeVariable(GuppyGameVariables.Scope.SceneType.Create(typeof(TScene)));
        }
    }
}
