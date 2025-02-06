using Guppy.Core.Common.Implementations;

namespace Guppy.Game.Graphics.Common.Constants
{
    public static class GuppyGraphicsVariables
    {
        public static class Scope
        {
            public class GraphicsEnabled(bool value = true) : ScopeVariable<GraphicsEnabled, bool>(value)
            {

            }
        }
    }
}
