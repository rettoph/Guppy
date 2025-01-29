using Guppy.Core.Common.Implementations;

namespace Guppy.Game.Common.Constants
{
    public static class GuppyGameVariables
    {
        public static class Scope
        {
            public class SceneType(Type? value) : ScopeVariable<SceneType, Type?>(value)
            {
                public override bool Matches(SceneType value)
                {
                    if (this.Value is null)
                    {
                        return value.Value is null;
                    }

                    return this.Value.IsAssignableTo(value.Value);
                }
            }
        }
    }
}
