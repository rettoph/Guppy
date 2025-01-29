using Guppy.Core.Common.Implementations;

namespace Guppy.Game.MonoGame.Common.Constants
{
    public static class MonoGameVariables
    {
        public static class Scope
        {
            public class SceneHasDebugWindow(bool value) : ScopeVariable<SceneHasDebugWindow, bool>(value)
            {
            }

            public class SceneHasTerminalWindow(bool value) : ScopeVariable<SceneHasTerminalWindow, bool>(value)
            {
            }
        }
    }
}
