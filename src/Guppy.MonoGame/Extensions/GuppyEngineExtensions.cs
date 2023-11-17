using Guppy.MonoGame;
using Guppy.MonoGame.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        public static void Draw(this GuppyEngine engine, GameTime gameTime)
        {
            foreach(var guppy in engine.Guppies.Collection<IGuppyDrawable>())
            {
                guppy.Draw(gameTime);
            }
        }

        public static void Update(this GuppyEngine engine, GameTime gameTime)
        {
            foreach (var guppy in engine.Guppies.Collection<IGuppyUpdateable>())
            {
                guppy.Update(gameTime);
            }
        }
    }
}
