using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public abstract class FrameableGuppy : IGuppy
    {
        private World _world;

        public FrameableGuppy(World world)
        {
            _world = world;
        }

        public virtual void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }
    }
}
