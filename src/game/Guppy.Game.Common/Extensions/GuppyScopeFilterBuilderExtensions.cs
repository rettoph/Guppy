using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Common.Constants;

namespace Guppy.Game.Common.Extensions
{
    public static class GuppyScopeFilterBuilderExtensions
    {
        public static GuppyScopeFilterBuilder RequireScene(this GuppyScopeFilterBuilder filter, Type? sceneType)
        {
            if (sceneType is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);
            }

            return filter.RequireScopeVariable(GuppyGameVariables.Scope.SceneType.Create(sceneType));
        }

        public static GuppyScopeFilterBuilder RequireScene<TScene>(this GuppyScopeFilterBuilder filter)
        {
            return filter.RequireScopeVariable(GuppyGameVariables.Scope.SceneType.Create(typeof(TScene)));
        }
    }
}
