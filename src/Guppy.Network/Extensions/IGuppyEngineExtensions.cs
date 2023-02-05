using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Network.Loaders;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureNetwork(this GuppyEngine guppy)
        {
            if (guppy.HasTag(nameof(ConfigureNetwork)))
            {
                return guppy;
            }

            return guppy.AddServiceLoader(new NetworkServiceLoader())
                .AddTag(nameof(ConfigureNetwork));
        }
    }
}
