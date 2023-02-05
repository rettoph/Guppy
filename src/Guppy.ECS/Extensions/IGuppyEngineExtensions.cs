using Guppy.Common;
using Guppy.Common.Extensions;
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
            if(guppy.HasTag(nameof(ConfigureECS)))
            {
                return guppy;
            }

            return guppy.AddServiceLoader(new ECSLoader())
                .AddTag(nameof(ConfigureECS));
        }
    }
}
