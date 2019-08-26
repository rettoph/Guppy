using Guppy.Network.ServiceLoaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions
{
    public static class GuppyLoaderExtensions
    {
        public static GuppyLoader ConfigureNetwork(this GuppyLoader loader, String appIdentifier)
        {
            loader.AddServiceLoader(new NetworkServiceLoader(appIdentifier));

            return loader;
        }
    }
}
