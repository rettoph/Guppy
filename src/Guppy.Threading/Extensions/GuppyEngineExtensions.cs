using Guppy.Threading.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureThreading(this GuppyEngine guppy)
        {
            if (guppy.Tags.Contains(nameof(ConfigureThreading)))
            {
                return guppy;
            }

            return guppy.AddInitializer(new BusInitializer())
                .AddTag(nameof(ConfigureThreading));
        }
    }
}
