using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.MonoGame.UI.Loaders;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureUI(
            this GuppyEngine guppy)
        {
            if(guppy.HasTag(nameof(ConfigureUI)))
            {
                return guppy;
            }

            return guppy.AddServiceLoader(new UIServiceLoader())
                .AddTag(nameof(ConfigureUI));
        }
    }
}
