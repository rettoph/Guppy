using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using Guppy.ECS.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureECS(this GuppyConfiguration engine)
        {
            if(engine.HasTag(nameof(ConfigureECS)))
            {
                return engine;
            }

            return engine.AddServiceLoader(new ECSLoader())
                .AddTag(nameof(ConfigureECS));
        }
    }
}
