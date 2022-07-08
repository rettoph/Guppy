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

            return guppy.AddInitializer(new ResourcePackInitializer())
                .AddInitializer(new ResourceInitializer())
                .AddInitializer(new SettingInitializer())
                .AddTag(nameof(ConfigureResources));
        }
    }
}
