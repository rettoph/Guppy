using Guppy.EntityComponent.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Library
{
    public class Ball : Frameable
    {
        public Vector2 Position;

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
