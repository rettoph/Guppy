using Guppy.Core.Common.Builders;
using Guppy.Core.Network.Common.Extensions;

namespace Guppy.Game.Graphics.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterGraphicsEnabledFilter(this IGuppyScopeBuilder builder, bool graphicsEnabled, Action<IGuppyScopeBuilder> build)
        {
            return builder.Filter(filter => filter.RequireGraphicsEnabled(graphicsEnabled), build);
        }
    }
}