using Guppy.ECS.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureECS(this GuppyEngine guppy)
        {
            if(guppy.Tags.Contains(nameof(ConfigureECS)))
            {
                return guppy;
            }

            return guppy.AddLoader(new ECSLoader(), 0)
                .AddTag(nameof(ConfigureECS));
        }
    }
}
