using Guppy.Core.Common.Builders;
using Guppy.Game.MonoGame.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Extensions
{
    public static class IScopeVairableServiceBuilderExtensions
    {
        public static IScopeVariableServiceBuilder AddSceneHasDebugWindow(this IScopeVariableServiceBuilder builder, bool value)
        {
            return builder.Add(MonoGameVariables.Scope.SceneHasDebugWindow.Create(value));
        }

        public static IScopeVariableServiceBuilder AddSceneHasTerminalWindow(this IScopeVariableServiceBuilder builder, bool value)
        {
            return builder.Add(MonoGameVariables.Scope.SceneHasTerminalWindow.Create(value));
        }
    }
}