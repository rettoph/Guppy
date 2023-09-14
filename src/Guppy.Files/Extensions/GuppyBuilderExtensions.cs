using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using Guppy.Files.Loaders;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureFiles(this GuppyConfiguration builder)
        {
            if (builder.HasTag(nameof(ConfigureFiles)))
            {
                return builder;
            }

            return builder
                .AddServiceLoader(new FileLoader())
                .AddTag(nameof(ConfigureFiles));
        }
    }
}
