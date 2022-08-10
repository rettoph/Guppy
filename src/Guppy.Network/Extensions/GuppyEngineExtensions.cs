using Guppy.Network.Initializers;
using Guppy.Network.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureNetwork(this GuppyEngine guppy, byte channelsCount)
        {
            if (guppy.Tags.Contains(nameof(ConfigureNetwork)))
            {
                return guppy;
            }

            return guppy.AddInitializer(new NetworkInitializer())
                .AddLoader(new NetworkServiceLoader(channelsCount))
                .AddTag(nameof(ConfigureNetwork));
        }
    }
}
