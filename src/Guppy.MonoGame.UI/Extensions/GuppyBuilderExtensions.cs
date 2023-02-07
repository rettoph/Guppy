using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using Guppy.MonoGame.UI.Loaders;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureUI(this GuppyConfiguration builder)
        {
            if(builder.HasTag(nameof(ConfigureUI)))
            {
                return builder;
            }

            return builder.AddServiceLoader(new UIServiceLoader())
                .AddTag(nameof(ConfigureUI));
        }
    }
}
