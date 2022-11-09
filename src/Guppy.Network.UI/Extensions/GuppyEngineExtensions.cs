using Guppy.Network.UI.Loaders;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureNetworkUI(
            this GuppyEngine guppy)
        {
            if(guppy.Tags.Contains(nameof(ConfigureNetworkUI)))
            {
                return guppy;
            }

            return guppy.ConfigureUI()
                .AddLoader(new NetworkUIServiceLoader(), 0)
                .AddTag(nameof(ConfigureNetworkUI));
        }
    }
}
