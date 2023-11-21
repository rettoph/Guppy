using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Configurations;
using Guppy.GUI.Loaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        public static GuppyConfiguration ConfigureGui(this GuppyConfiguration builder)
        {
            if (builder.HasTag(nameof(ConfigureGui)))
            {
                return builder;
            }

            return builder
                .AddServiceLoader<GUILoader>()
                .AddTag(nameof(ConfigureGui));
        }
    }
}
