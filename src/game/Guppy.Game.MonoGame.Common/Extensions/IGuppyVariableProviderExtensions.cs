using Guppy.Core.Common;
using Guppy.Core.Common.Providers;
using Guppy.Game.MonoGame.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class IGuppyVariableProviderExtensions
    {
        public static bool GetSceneHasDebugWindow(this IGuppyVariableProvider<IScopeVariable> provider)
        {
            return provider.TryGet<MonoGameVariables.Scope.SceneHasDebugWindow>(out var variable) && variable.Value;
        }

        public static bool GetSceneHasTerminalWindow(this IGuppyVariableProvider<IScopeVariable> provider)
        {
            return provider.TryGet<MonoGameVariables.Scope.SceneHasTerminalWindow>(out var variable) && variable.Value;
        }
    }
}
