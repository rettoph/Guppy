using Guppy.MonoGame;
using Guppy.MonoGame.Initializers;
using Guppy.MonoGame.Loaders;
using Guppy.MonoGame.UI.Loaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static GuppyEngine ConfigureUI(
            this GuppyEngine guppy)
        {
            if(guppy.Tags.Contains(nameof(ConfigureUI)))
            {
                return guppy;
            }

            return guppy.AddLoader(new UIServiceLoader())
                .AddTag(nameof(ConfigureUI));
        }
    }
}
