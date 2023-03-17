using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using Guppy.GUI.Loaders;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureGUI(this GuppyConfiguration builder)
        {
            if(builder.HasTag(nameof(ConfigureGUI)))
            {
                return builder;
            }

            return builder.AddServiceLoader(new GUIServiceLoader())
                .AddTag(nameof(ConfigureGUI));
        }
    }
}
