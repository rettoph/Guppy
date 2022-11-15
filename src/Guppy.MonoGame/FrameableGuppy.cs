using Guppy.Common;
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
        public readonly IGameComponentService Components;

        public FrameableGuppy(IGameComponentService components)
        {
            this.Components = components;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Components.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            this.Components.Draw(gameTime);
        }
    }
}
