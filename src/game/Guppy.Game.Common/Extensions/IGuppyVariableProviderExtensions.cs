using Guppy.Core.Common;
using Guppy.Core.Common.Providers;
using Guppy.Game.Common.Constants;

namespace Guppy.Game.Common.Extensions
{
    public static class IGuppyVariableProviderExtensions
    {
        public static Type? GetSceneType(this IGuppyVariableProvider<IScopeVariable> provider)
        {
            return provider.TryGet<GuppyGameVariables.Scope.SceneType>(out var variable) ? variable.Value : default;
        }
    }
}
