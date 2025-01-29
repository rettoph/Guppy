using Guppy.Core.Common;
using Guppy.Game.MonoGame.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder AddSceneHasDebugWindow(this IGuppyScopeBuilder builder, bool value)
        {
            return builder.AddScopeVariable(MonoGameVariables.Scope.SceneHasDebugWindow.Create(value));
        }

        public static IGuppyScopeBuilder AddSceneHasTerminalWindow(this IGuppyScopeBuilder builder, bool value)
        {
            return builder.AddScopeVariable(MonoGameVariables.Scope.SceneHasTerminalWindow.Create(value));
        }
    }
}