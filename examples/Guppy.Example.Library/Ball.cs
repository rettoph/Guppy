using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Library
{
    public class Ball : Frameable
    {
        public Vector2 Position;

        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
