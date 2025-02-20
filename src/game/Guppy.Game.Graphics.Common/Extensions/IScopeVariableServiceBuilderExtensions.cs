using Guppy.Core.Common.Builders;
using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Game.Graphics.Common.Extensions
{
    public static class IScopeVariableServiceBuilderExtensions
    {
        public static IScopeVariableServiceBuilder AddGraphicsEnabled(this IScopeVariableServiceBuilder builder, bool value)
        {
            return builder.Add(GuppyGraphicsVariables.Scope.GraphicsEnabled.Create(value));
        }
    }
}
