using Guppy.Commands.Loaders;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureCommands(this GuppyConfiguration builder)
        {
            if (builder.HasTag(nameof(ConfigureCommands)))
            {
                return builder;
            }

            return builder.AddServiceLoader(new CommandLoader())
                .AddTag(nameof(ConfigureCommands));
        }
    }
}
