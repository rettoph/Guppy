using Guppy.Configurations;
using Guppy.Input.Loaders;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureInput(this GuppyConfiguration builder)
        {
            if (builder.HasTag(nameof(ConfigureInput)))
            {
                return builder;
            }

            return builder.ConfigureECS()
                .AddServiceLoader(new InputLoader())
                .AddTag(nameof(ConfigureInput));
        }
    }
}
