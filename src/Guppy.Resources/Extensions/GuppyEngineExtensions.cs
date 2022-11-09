using Guppy.Resources.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureResources(this GuppyEngine guppy)
        {
            if (guppy.Tags.Contains(nameof(ConfigureResources)))
            {
                return guppy;
            }

            return guppy.AddInitializer(new ResourcePackInitializer(), 0)
                .AddInitializer(new ResourceInitializer(), 0)
                .AddInitializer(new SettingInitializer(), 0)
                .AddTag(nameof(ConfigureResources));
        }
    }
}
