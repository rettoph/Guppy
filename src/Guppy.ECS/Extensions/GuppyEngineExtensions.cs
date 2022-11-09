using Guppy.ECS.Initializers;
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

            return guppy.AddInitializer(new ECSInitializer(), 0)
                .AddTag(nameof(ConfigureECS));
        }
    }
}
