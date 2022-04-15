using Guppy.EntityComponent.Initializers;
using Guppy.EntityComponent.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureEntityComponent(this GuppyEngine guppy)
        {
            if (guppy.Tags.Contains(nameof(ConfigureEntityComponent)))
            {
                return guppy;
            }

            return guppy.AddInitializer(new ComponentInitializer())
                .AddInitializer(new SetupInitializer())
                .AddLoader(new EntityComponentServiceLoader())
                .AddTag(nameof(ConfigureEntityComponent));
        }
    }
}
