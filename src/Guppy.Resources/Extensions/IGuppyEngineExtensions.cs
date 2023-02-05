using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Resources.Loaders;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureResources(this GuppyEngine guppy)
        {
            if (guppy.HasTag(nameof(ConfigureResources)))
            {
                return guppy;
            }

            return guppy
                .AddServiceLoader(new SettingLoader())
                .AddServiceLoader(new ResourceLoader())
                .AddTag(nameof(ConfigureResources));
        }
    }
}
