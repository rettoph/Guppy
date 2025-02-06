using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class GuppyScopeFilterBuilderExtensions
    {
        public static GuppyScopeFilterBuilder RequireGraphicsEnabled(this GuppyScopeFilterBuilder builder, bool peerType)
        {
            return builder.RequireScopeVariable(GuppyGraphicsVariables.Scope.GraphicsEnabled.Create(peerType));
        }
    }
}
