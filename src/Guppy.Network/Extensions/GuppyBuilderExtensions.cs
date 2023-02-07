using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using Guppy.Network.Loaders;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureNetwork(this GuppyConfiguration builder)
        {
            if (builder.HasTag(nameof(ConfigureNetwork)))
            {
                return builder;
            }

            return builder.AddServiceLoader(new NetworkServiceLoader())
                .AddTag(nameof(ConfigureNetwork));
        }
    }
}
