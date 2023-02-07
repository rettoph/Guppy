using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using Guppy.Network.UI.Loaders;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureNetworkUI(this GuppyConfiguration builder)
        {
            if(builder.HasTag(nameof(ConfigureNetworkUI)))
            {
                return builder;
            }

            return builder.ConfigureUI()
                .AddServiceLoader(new NetworkUIServiceLoader())
                .AddTag(nameof(ConfigureNetworkUI));
        }
    }
}
