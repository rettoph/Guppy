using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Network.UI.Loaders;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureNetworkUI(
            this GuppyEngine guppy)
        {
            if(guppy.HasTag(nameof(ConfigureNetworkUI)))
            {
                return guppy;
            }

            return guppy.ConfigureUI()
                .AddServiceLoader(new NetworkUIServiceLoader())
                .AddTag(nameof(ConfigureNetworkUI));
        }
    }
}
