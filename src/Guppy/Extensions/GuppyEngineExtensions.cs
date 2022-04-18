using Guppy.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureSettings(this GuppyEngine guppy)
        {
            if (guppy.Tags.Contains(nameof(ConfigureSettings)))
            {
                return guppy;
            }

            return guppy.AddInitializer(new SettingInitializer())
                .AddTag(nameof(ConfigureSettings));
        }
    }
}
