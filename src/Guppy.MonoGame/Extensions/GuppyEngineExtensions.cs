using Guppy.MonoGame;
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
            foreach(var guppy in engine.Guppies.Collection<IDrawable>())
            {
                guppy.Draw(gameTime);
            }
        }

        public static void Update(this GuppyEngine engine, GameTime gameTime)
        {
            foreach (var guppy in engine.Guppies.Collection<IUpdateable>())
            {
                guppy.Update(gameTime);
            }
        }
    }
}
