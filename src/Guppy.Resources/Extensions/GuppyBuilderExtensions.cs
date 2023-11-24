using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using Guppy.Resources.Loaders;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureResources(this GuppyConfiguration builder)
        {
            if (builder.HasTag(nameof(ConfigureResources)))
            {
                return builder;
            }

            return builder
                .AddServiceLoader<ServiceLoader>()
                .ConfigureFiles()
                .AddTag(nameof(ConfigureResources));
        }
    }
}
