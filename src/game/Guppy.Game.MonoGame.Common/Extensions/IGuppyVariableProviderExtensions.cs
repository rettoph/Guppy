using Guppy.Core.Common;
using Guppy.Core.Common.Providers;
using Guppy.Game.MonoGame.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class IGuppyVariableProviderExtensions
    {
        public static bool GetSceneHasDebugWindow(this IGuppyVariableProvider<IScopeVariable> provider)
        {
            return provider.GetVariable<MonoGameVariables.Scope.SceneHasDebugWindow>()?.Value ?? false;
        }

        public static bool GetSceneHasTerminalWindow(this IGuppyVariableProvider<IScopeVariable> provider)
        {
            return provider.GetVariable<MonoGameVariables.Scope.SceneHasTerminalWindow>()?.Value ?? false;
        }
    }
}
