using Guppy.MonoGame.Services;
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
        public readonly World World;

        public FrameableGuppy(World world)
        {
            this.World = world;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.World.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            this.World.Draw(gameTime);
        }
    }
}
