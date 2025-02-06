using Guppy.Core.Common;
using Guppy.Core.Network.Common.Extensions;
using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Game.Graphics.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder AddGraphicsEnabled(this IGuppyScopeBuilder builder, bool value)
        {
            return builder.AddScopeVariable(GuppyGraphicsVariables.Scope.GraphicsEnabled.Create(value));
        }

        public static IGuppyScopeBuilder RegisterGraphicsEnabledFilter(this IGuppyScopeBuilder builder, bool graphicsEnabled, Action<IGuppyScopeBuilder> build)
        {
            return builder.Filter(filter => filter.RequireGraphicsEnabled(graphicsEnabled), build);
        }
    }
}